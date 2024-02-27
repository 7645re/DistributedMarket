using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Migrator;

public static class MigrationRunner
{
    public static void Migrate(DatabaseType databaseType, Assembly assembly, string? databaseName = null)
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
        
        if (databaseType == DatabaseType.Mssql && databaseName != null)
            EnsureDatabase(databaseOptions, databaseName);
        
        var serviceProvider = CreateServices(databaseOptions.ConnectionString, assembly);
        using var scope = serviceProvider.CreateScope();
        scope.ServiceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();        
    }

    private static void EnsureDatabase(DatabaseOptions databaseOptions, string databaseName)
    {
        using var connection = new SqlConnection(
            databaseOptions.ConnectionString.Replace($"Database={databaseName};", ""));
        connection.Open();

        var sqlCommand = $"IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = '{databaseName}') CREATE DATABASE {databaseName};";
        try
        {
            using (var command = new SqlCommand(sqlCommand))
            {
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
            Console.WriteLine("Creation of the database was successful");
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to create database");
            Console.WriteLine(e);
            throw;
        }
    }

    private static IServiceProvider CreateServices(string connectionString, Assembly assembly)
    {
        return new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(assembly)
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