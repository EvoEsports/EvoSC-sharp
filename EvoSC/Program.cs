using System;
using EvoSC.Migrations;
using FluentMigrator.Runner;

using Microsoft.Extensions.DependencyInjection;

namespace EvoSC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, EvoSC!");
            
            Console.WriteLine("Creating database \"evosc\"...");
            var serviceProvider = CreateServices();

            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
            
            Console.WriteLine("Done!");
            
        }

        private static IServiceProvider CreateServices()
        {
            const string connectionString = "server=localhost;uid=evosc;pwd=evosc123!;database=evosc;SslMode=none";
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddMySql5()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(CreateDatabase).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            runner.MigrateUp();
        }
    }
}
