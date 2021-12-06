using EvoSC.Core.Configuration;
using EvoSC.Modules.DatabaseConnection.Injections;
using EvoSC.Modules.DatabaseConnection.Systems;
using FluentMigrator.Runner;
using GameHost.V3;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.Module;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace EvoSC.Modules.DatabaseConnection
{
    public class Module : HostModule
    {
        private readonly HostRunnerScope _hostScope;

        private static ILogger s_logger;

        private ControllerConfig _controllerConfig;
        
        public Module(HostRunnerScope scope) : base(scope)
        {
            _hostScope = scope;
            
            Dependencies.AddRef(() => ref _controllerConfig);
            Dependencies.AddRef(() => ref s_logger);
        }

        protected override void OnInit()
        {
            const string ConnectionString = "server=localhost;uid=evosc;pwd=evosc123;database=evosc;SslMode=none";
            
            // Register a dynamic dependency that will create an IMigrationRunner from the assembly of the caller
            _hostScope.Context.Register(typeof(IMigrationRunner), new TransientMigrationRunner(assembly =>
            {
                s_logger.Info("Initializing database for {0}", assembly.GetName());
            
                // Moved CreateServices into a local function (since it will only be used here)
                ServiceProvider CreateServices()
                {
                    return new ServiceCollection()
                        .AddFluentMigratorCore()
                        .ConfigureRunner(rb => rb
                            .AddMySql5()
                            .WithGlobalConnectionString(ConnectionString)
                            .ScanIn(assembly).For.Migrations())
                        .AddLogging(lb => lb.AddFluentMigratorConsole())
                        .BuildServiceProvider(false);
                }
                
                var serviceCollection = CreateServices();
                // Directly create the service one time and get back the IMigrationRunner
                return serviceCollection.GetRequiredService<IMigrationRunner>();
            }));

            // Create a default system that will automatically migrate on the definition provided by the assembly
            // (aka the files in Migrations/*.cs)
            Disposables.Add(new DefaultMigrateSystem(ModuleScope));
        }
    }
}
