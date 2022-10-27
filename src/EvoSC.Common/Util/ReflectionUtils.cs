using System.Reflection;
using EvoSC.Common.Interfaces.Controllers;

namespace EvoSC.Common.Util;

public static class ReflectionUtils
{
    /// <summary>
    /// Binding flags for public declared methods of a type that can be instantiated.
    /// </summary>
    public const BindingFlags InstanceMethods = BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public;

    public static bool IsControllerClass(this Type controllerType)
    {
        foreach (var intf in controllerType.GetInterfaces())
        {
            if (intf == typeof(IController))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Get a default non-null value of a given type. If the type is a custom type,
    /// the method assumes a default constructor exists.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static object GetDefaultTypeValue(this Type t)
    {
        if (t == typeof(string))
        {
            return "";
        }
        
        if (t == typeof(int) || t == typeof(uint) || t == typeof(short) || t == typeof(ushort) ||
            t == typeof(long) || t == typeof(ulong))
        {
            return 0;
        } 
        
        if (t == typeof(float) || t == typeof(double))
        {
            return 0.0;
        }

        if (t.GetInterfaces().FirstOrDefault(ti => ti.IsGenericType && ti.GetGenericTypeDefinition().IsAssignableTo(typeof(IEnumerable<>))) != null)
        {
            return Array.CreateInstance(t, 0);
        }
        
        return Activator.CreateInstance(t);
    }
}
