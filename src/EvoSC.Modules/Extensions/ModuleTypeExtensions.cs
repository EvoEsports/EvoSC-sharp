using System.Reflection;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Extensions;

public static class ModuleTypeExtensions
{
    /// <summary>
    /// Get the module attribute for a type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static ModuleAttribute? GetModuleAttribute(this Type type) => type.GetCustomAttribute<ModuleAttribute>();

    /// <summary>
    /// Check whether a type is a module class.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsEvoScModuleType(this Type type) => type.IsClass && type.IsAssignableTo(typeof(IEvoScModule)) && type.GetModuleAttribute() != null;
}
