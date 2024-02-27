using System.Reflection;
using Shared.Migrator;

MigrationRunner.Migrate(DatabaseType.Postgresql, Assembly.GetExecutingAssembly());