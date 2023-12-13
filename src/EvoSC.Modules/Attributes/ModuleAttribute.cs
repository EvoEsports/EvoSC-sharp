namespace EvoSC.Modules.Attributes;

/// <summary>
/// Defines a class as a module's main class.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ModuleAttribute : Attribute
{
    /// <summary>
    /// Whether this is an internal module or not.
    /// </summary>
    public bool IsInternal { get; init; }
}
