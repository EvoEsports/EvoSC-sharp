using System;
using EvoSC.Core.Configuration;
using EvoSC.Migrations;
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

        private static ILogger _logger;

        private ControllerConfig _controllerConfig;
        
        public Module(HostRunnerScope scope) : base(scope)
        {
            _hostScope = scope;
            
            Dependencies.AddRef(() => ref _controllerConfig);
            Dependencies.AddRef(() => ref _logger);
        }

        protected override void OnInit()
        {
            var serviceCollection = CreateServices();
            
            _hostScope.Context.Register(serviceCollection);
        }

        private static IServiceProvider CreateServices()
        {
            _logger.Info("Initializing database");
            const string ConnectionString = "server=localhost;uid=evosc;pwd=evosc123!;database=evosc;SslMode=none";
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddMySql5()
                    .WithGlobalConnectionString(ConnectionString)
                    .ScanIn(typeof(CreateDatabase).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }
    }
}
