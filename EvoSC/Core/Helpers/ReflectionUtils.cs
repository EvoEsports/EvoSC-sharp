using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Core.Helpers;

public static class ReflectionUtils
{
    /// <summary>
    /// Execute a method with DI.
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="method"></param>
    /// <param name="services"></param>
    /// <param name="pars"></param>
    /// <returns></returns>
    public static object ExecuteMethod(object instance, MethodInfo method, IServiceProvider services, params object[] pars)
    {
        List<object> args = new List<object>(pars);

        var serviceParams = method.GetParameters()[pars.Length..];

        foreach (var param in serviceParams)
        {
            var service = services.GetRequiredService(param.ParameterType);
            args.Add(service);
        }

        return method.Invoke(instance, args.ToArray());
    }

    public static MethodInfo GetInstanceMethod(object obj, string name)
    {
        var type = obj.GetType();
        var method = type.GetMethod(name, BindingFlags.Instance | BindingFlags.Public);
        return method;
    }
}
