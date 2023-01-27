using System.Reflection;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.BatchParser;
using FluentMigrator.Runner.Generators.SQLite;
using FluentMigrator.Runner.Processors.SQLite;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Common.Tests.Database;

public static class TestDbMigrations
{
    public static void MigrateFromAssembly(string connectionString, Assembly asm)
    {
        var services = new ServiceCollection();
        var sp = services.AddFluentMigratorCore()
            .ConfigureRunner(r => r
                .AddSQLite()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(asm).For.Migrations()
            )
            .BuildServiceProvider();

        var runner = sp.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}
