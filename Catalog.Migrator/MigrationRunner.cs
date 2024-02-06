using Catalog.Migrator.Migrations;
using Catalog.Migrator.Options;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Migrator;

public static class MigrationRunner
{
    public static void Migrate()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
        
        var databaseOptions = configurationBuilder
            .Build()
            .GetRequiredSection("Database")
            .Get<DatabaseOptions>()!;
        
        EnsureDatabase(databaseOptions);
        
        var serviceProvider = CreateServices(databaseOptions!.ConnectionString);
        using var scope = serviceProvider.CreateScope();
        scope.ServiceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();        
    }

    private static void EnsureDatabase(DatabaseOptions databaseOptions)
    {
        using var connection = new SqlConnection(databaseOptions.ConnectionString.Replace("Database=Catalog;", ""));
        connection.Open();

        const string sqlCommand = "IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'Catalog') CREATE DATABASE Catalog;";
        try
        {
            using (var command = new SqlCommand(sqlCommand))
            {
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
            Console.WriteLine("Сreation of the database was successful");
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to create database");
            Console.WriteLine(e);
            throw;
        }
    }

    private static IServiceProvider CreateServices(string connectionString)
    {
        return new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(CreateProductAndCategoryTables).Assembly)
                .For
                .Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .Configure<RunnerOptions>(opt =>
            {
                opt.TransactionPerSession = false;
            })
            .BuildServiceProvider(false);
    }
}