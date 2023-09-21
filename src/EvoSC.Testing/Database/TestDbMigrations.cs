using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Testing.Database;

public static class TestDbMigrations
{
    /// <summary>
    /// Run all migrations from an assembly.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="asm"></param>
    public static void MigrateFromAssembly(string connectionString, Assembly asm)
    {
        var services = new ServiceCollection();
        var sp = services.AddFluentMigratorCore()
            .ConfigureRunner(r => r
                .AddSQLite()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(asm).For.Migrations()
            ).Configure<RunnerOptions>(opt =>
            {
                opt.Tags = new[] { "Production" };
            })
            .BuildServiceProvider();

        var runner = sp.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}
