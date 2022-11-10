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

    /// <summary>
    /// Create an instance of a type using the provided generic type.
    /// </summary>
    /// <param name="genericType">Type of the generic parameter.</param>
    /// <param name="t">Type of the object to instantiate.</param>
    /// <param name="args">Arguments to pass to the constructor.</param>
    /// <returns></returns>
    public static object CreateGenericInstance(Type genericType, Type t, params object[] args)
    {
        var genericTypeType = genericType.MakeGenericType(t);
        return Activator.CreateInstance(genericTypeType, args);
    }

    /// <summary>
    /// Call a method on an instance by using a string name representing the name of the method.
    /// </summary>
    /// <param name="type">The type of the object's instance.</param>
    /// <param name="instance">The instance of the object type</param>
    /// <param name="methodName">The name of the method to call.</param>
    /// <param name="args">Arguments to pass to the method.</param>
    /// <returns></returns>
    public static object CallMethod(Type type, object instance, string methodName, params object[] args)
    {
        var method = type.GetMethod(methodName);
        return method.Invoke(instance, args);
    }
    
    /// <summary>
    /// Call a method on an instance by using a string name representing the name of the method.
    /// </summary>
    /// <param name="instance">Object instance that contains the method to call.</param>
    /// <param name="methodName">Name of the method to call.</param>
    /// <param name="args">Arguments to pass to the method.</param>
    /// <returns></returns>
    public static object CallMethod(object instance, string methodName, params object[] args)
    {
        var method = instance.GetType().GetMethod(methodName);
        return method.Invoke(instance, args);
    }

    /// <summary>
    /// Call a static method on a type by using a string name representation of the method's name.
    /// </summary>
    /// <param name="type">The type to call the method on.</param>
    /// <param name="methodName">Name of the method to call.</param>
    /// <param name="args">Arguments to pass to the method.</param>
    /// <returns></returns>
    public static object CallStaticMethod(Type type, string methodName, params object[] args)
    {
        var method = type.GetMethod(methodName);
        return method.Invoke(null, args);
    }
}
