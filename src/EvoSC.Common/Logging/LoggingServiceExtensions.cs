using System.Globalization;
using EvoSC.Common.Config.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
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
    }

    private static LogLevel GetLogLevel(this ILoggingConfig config) =>
        config.LogLevel.ToLower(CultureInfo.InvariantCulture) switch
        {
            "information" => LogLevel.Information,
            "critical" => LogLevel.Critical,
            "debug" => LogLevel.Debug,
            "error" => LogLevel.Error,
            "none" => LogLevel.None,
            "trace" => LogLevel.Trace,
            "warning" => LogLevel.Warning,
            _ => LogLevel.Information
        };
}
