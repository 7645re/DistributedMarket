using System.Reflection;
using Shared.Migrator;

MigrationRunner.Migrate(DatabaseType.Mssql, Assembly.GetExecutingAssembly(), "Catalog");