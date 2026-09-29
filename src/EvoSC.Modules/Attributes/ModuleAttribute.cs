namespace EvoSC.Modules.Attributes;

/// <summary>
/// Defines a class as a module's main class.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ModuleAttribute : Attribute
{
    /// <summary>
    /// Whether this module is internal, meaning it is part of the application itself: the
    /// application registers it and loads it from the internal module directory, and it is never
    /// unloaded or reloaded at run time.
    /// </summary>
    public bool IsInternal { get; init; }
}
