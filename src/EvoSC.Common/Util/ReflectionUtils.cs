using System.Reflection;

namespace EvoSC.Common.Util;

public static class ReflectionUtils
{
    /// <summary>
    /// Binding flags for public declared methods of a type that can be instantiated.
    /// </summary>
    public const BindingFlags InstanceMethods = BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public;
}
