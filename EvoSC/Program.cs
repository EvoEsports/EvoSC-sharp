using System;
using EvoSC.Contracts;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace EvoSC
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Info("Initializing EvoSC...");
            Console.WriteLine("Initializing EvoSC...");
            
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }

            logger.Info("Completed initialization");
            Console.WriteLine("Completed initialization");

            var sample = host.Services.GetRequiredService<ISampleService>();
            logger.Info(sample.GetName());
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog(); // NLog: Setup NLog for Dependency injection

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            runner.MigrateUp();
        }
    }
}
