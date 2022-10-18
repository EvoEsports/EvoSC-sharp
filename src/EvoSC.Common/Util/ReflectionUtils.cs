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
}
