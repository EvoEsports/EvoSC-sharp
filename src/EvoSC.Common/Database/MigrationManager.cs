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
    private readonly IEvoSCBaseConfig _config;
    
    public MigrationManager(IEvoSCBaseConfig config)
    {
        _config = config;
    }
    
    public void MigrateFromAssembly(Assembly asm)
    {
        var provider = new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(c => c
                .AddMySql5()
                .WithGlobalConnectionString(_config.Database.GetConnectionString())
                .ScanIn(asm).For.Migrations())
            .AddEvoScLogging(_config.Logging)
            .BuildServiceProvider(false);

        provider.GetRequiredService<IMigrationRunner>()
            .MigrateUp();
        
        provider.Dispose();
    }
}
