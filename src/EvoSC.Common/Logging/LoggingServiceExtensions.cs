using System.Text;
using System.Text.Json;
using EvoSC.Common.Config.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using SimpleInjector;

namespace EvoSC.Common.Logging;

public static class LoggingServiceExtensions
{
    /// <summary>
    /// Set up logging in the service container.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="isDebug"></param>
    /// <returns></returns>
    public static Container AddEvoScLogging(this Container services, ILoggingConfig config)
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(config.GetLogLevel());

            if (config.UseJson)
            {
                builder.AddJsonConsole(o =>
                {
                    o.IncludeScopes = true;
                    o.TimestampFormat = "dd.MM.yyyy hh:mm:ss.ffff";
                    o.UseUtcTimestamp = true;
                });
            }
            else
            {
                builder.AddSimpleConsole(c =>
                {
                    c.ColorBehavior = LoggerColorBehavior.Enabled;
                    c.SingleLine = true;
                    c.TimestampFormat = "[dd.MM.yyyy hh:mm:ss.ffff] ";
                });
            }
        });
        
        
        services.RegisterInstance<ILoggerFactory>(loggerFactory);
        services.RegisterSingleton(typeof(ILogger<>), typeof(Logger<>));
        
        return services;
    }

    /// <summary>
    /// Add logging to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddEvoScLogging(this IServiceCollection services, ILoggingConfig config)
    {
        return services.AddLogging(builder =>
        {
            var logLevel = config.GetLogLevel();
            
            builder.ClearProviders();
            //builder.AddFilter(level => level == logLevel);
            builder.SetMinimumLevel(config.GetLogLevel());

            if (config.UseJson)
            {
                builder.AddJsonConsole(o =>
                {
                    o.IncludeScopes = true;
                    o.TimestampFormat = "dd.MM.yyyy hh:mm:ss.ffff";
                    o.UseUtcTimestamp = true;
                });
            }
            else
            {
                builder.AddSimpleConsole(c =>
                {
                    c.ColorBehavior = LoggerColorBehavior.Enabled;
                    c.SingleLine = true;
                    c.TimestampFormat = "[dd.MM.yyyy hh:mm:ss.ffff] ";
                });
            }
        });
    }

    private static LogLevel GetLogLevel(this ILoggingConfig config)
    {
        switch (config.LogLevel.ToLower())
        {
            case "information":
                return LogLevel.Information;
            case "critical":
                return LogLevel.Critical;
            case "debug":
                return LogLevel.Debug;
            case "error":
                return LogLevel.Error;
            case "none":
                return LogLevel.None;
            case "trace":
                return LogLevel.Trace;
            case "warning":
                return LogLevel.Warning;
            default:
                return LogLevel.Information;
        }
    }
}
