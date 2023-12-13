using System.Reflection;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Database.Extensions;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Logging;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Generators;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Common.Database;

public class MigrationManager(IEvoScBaseConfig config) : IMigrationManager
{
    public void MigrateFromAssembly(Assembly asm)
    {
        var provider = new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(c => c
                .AddMySql5()
                .AddSQLite()
                .AddPostgres()
                .WithGlobalConnectionString(config.Database.GetConnectionString())
                .ScanIn(asm).For.Migrations())
            .Configure<SelectingGeneratorAccessorOptions>(x =>
                x.GeneratorId = GetDatabaseTypeIdentifier(config.Database.Type))
            .AddEvoScLogging(config.Logging)
            .Configure<RunnerOptions>(opt =>
            {
                opt.Tags = new[] { "Production" };
            })
            .BuildServiceProvider(false);

        provider.GetRequiredService<IMigrationRunner>()
            .MigrateUp();

        provider.Dispose();
    }

    private static string GetDatabaseTypeIdentifier(IDatabaseConfig.DatabaseType databaseType) => databaseType switch
    {
        IDatabaseConfig.DatabaseType.MySql => "MySql5",
        IDatabaseConfig.DatabaseType.SQLite => "Sqlite",
        _ => "Postgres"
    };
}
