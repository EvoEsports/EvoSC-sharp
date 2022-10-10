using System.Reflection;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Logging;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Database;

public class MigrationManager : IMigrationManager
{
    private readonly DatabaseConfig _dbConfig;
    private readonly LoggingConfig _loggingConfig;
    
    public MigrationManager(DatabaseConfig dbConfig, LoggingConfig loggingConfig)
    {
        _dbConfig = dbConfig;
        _loggingConfig = loggingConfig;
    }
    
    public void MigrateFromAssembly(Assembly asm)
    {
        new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(c => c
                .AddMySql5()
                .WithGlobalConnectionString(_dbConfig.GetConnectionString())
                .ScanIn(asm).For.Migrations())
            .AddEvoScLogging(_loggingConfig)
            .BuildServiceProvider(false)
            .GetRequiredService<IMigrationRunner>()
            .MigrateUp();
    }
}
