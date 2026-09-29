using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Castle.DynamicProxy;
using Config.Net;
using EvoSC.Modules.Exceptions;

namespace EvoSC.Modules.Util;

/// <summary>
/// Builds the Config.Net interceptor chain (value parsing, store IO and property mapping) for a
/// settings interface.
/// </summary>
/// <remarks>
/// Config.Net's own <c>Build()</c> always emits the implementation with Castle DynamicProxy, which
/// cannot be used for a module's settings: Castle emits proxies into a non-collectible assembly,
/// while a module's settings interface lives in the module's collectible load context, and the CLR
/// rejects a non-collectible assembly referencing a collectible one. The interceptor is therefore
/// built here and handed to <see cref="CollectibleConfigProxy"/>, which emits into an assembly that
/// is collectible. The configuration itself stays Config.Net's, so value parsing, defaults and the
/// stores are handled exactly as before.
/// </remarks>
internal static class ConfigNetInterceptor
{
    private const string ValueHandlerTypeName = "Config.Net.Core.ValueHandler";
    private const string IoHandlerTypeName = "Config.Net.Core.IoHandler";
    private const string InterfaceInterceptorTypeName = "Config.Net.Core.InterfaceInterceptor";

    private const string StoresFieldName = "_stores";
    private const string ParsersFieldName = "_customParsers";
    private const string CacheIntervalFieldName = "_cacheInterval";

    /// <summary>
    /// Creates the interceptor for <paramref name="settingsInterface"/>, configured with the stores
    /// and type parsers of <paramref name="configurationBuilder"/>.
    /// </summary>
    internal static IInterceptor Create(object configurationBuilder, Type settingsInterface)
    {
        var stores = ReadField<IEnumerable>(configurationBuilder, StoresFieldName);
        var parsers = ReadField<IEnumerable>(configurationBuilder, ParsersFieldName);
        var cacheInterval = ReadField<TimeSpan>(configurationBuilder, CacheIntervalFieldName);

        var valueHandler = Construct(ValueHandlerTypeName, parsers);
        var ioHandler = Construct(IoHandlerTypeName, stores, valueHandler, cacheInterval);

        return (IInterceptor)Construct(InterfaceInterceptorTypeName, settingsInterface, ioHandler, null);
    }

    private static object ReadField<T>(object instance, string fieldName)
    {
        var field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                    ?? throw new EvoScModuleException(
                        $"Config.Net's configuration builder no longer has the field '{fieldName}'. " +
                        "A module's settings cannot be built with this version of Config.Net.");

        if (field.GetValue(instance) is not T value)
        {
            throw new EvoScModuleException(
                $"Config.Net's configuration builder field '{fieldName}' is not a {typeof(T).Name}.");
        }

        return value;
    }

    private static object Construct(string typeName, params object?[] arguments)
    {
        var type = typeof(ConfigurationBuilder<>).Assembly.GetType(typeName)
                   ?? throw new EvoScModuleException(
                       $"Config.Net no longer has the type '{typeName}'. A module's settings cannot be built " +
                       "with this version of Config.Net.");

        var constructor = Array.Find(type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic),
            ctor => ctor.GetParameters().Length == arguments.Length);

        if (constructor is null)
        {
            throw new EvoScModuleException(
                $"Config.Net's '{typeName}' has no constructor taking {arguments.Length} argument(s).");
        }

        try
        {
            return constructor.Invoke(arguments);
        }
        catch (TargetInvocationException e) when (e.InnerException is not null)
        {
            // Re-throw the original exception, preserving its stack trace.
            ExceptionDispatchInfo.Capture(e.InnerException).Throw();
            throw;
        }
    }
}
