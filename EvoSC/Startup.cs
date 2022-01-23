using System;
using EvoSC.Core.PluginHandler;
using EvoSC.Migrations;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EvoSC
{
    public class Startup
    {
        const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddMySql5()
                    .WithGlobalConnectionString(Environment.GetEnvironmentVariable("DOTNET_CONNECTION_STRING"))
                    .ScanIn(typeof(CreateDatabase).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .AddPluginLoaders();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EvoSC API v1"));
            }

            app.UseStaticFiles();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
