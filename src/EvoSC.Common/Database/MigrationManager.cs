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

public class MigrationManager : IMigrationManager
{
    private readonly IEvoScBaseConfig _config;

    public MigrationManager(IEvoScBaseConfig config)
    {
        _config = config;
    }

    public void MigrateFromAssembly(Assembly asm)
    {
        var provider = new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(c => c
                .AddMySql5()
                .AddSQLite()
                .AddPostgres()
                .WithGlobalConnectionString(_config.Database.GetConnectionString())
                .ScanIn(asm).For.Migrations())
            .Configure<SelectingGeneratorAccessorOptions>(x =>
                x.GeneratorId = GetDatabaseTypeIdentifier(_config.Database.Type))
            .AddEvoScLogging(_config.Logging)
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
