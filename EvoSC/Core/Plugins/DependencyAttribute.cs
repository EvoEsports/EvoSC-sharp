using System;
using JetBrains.Annotations;

namespace EvoSC.Core.Plugins
{
    // reflection D:

    /// <summary>
    ///     Add this attribute to register a property as a dependency
    /// </summary>
    /// <remarks>
    ///     This attribute only work system inheriting <see cref="PluginSystemBase" />
    /// </remarks>
    [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
    public class DependencyAttribute : Attribute
    {
    }
}