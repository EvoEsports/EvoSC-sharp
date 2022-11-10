namespace EvoSC.Modules.Attributes;

/// <summary>
/// Defines a class as a module's main class.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ModuleAttribute : Attribute
{
    /// <summary>
    /// Unique name of the module.
    /// </summary>
    public required string Name { get; init; }
    /// <summary>
    /// A summary of what the module is and does.
    /// </summary>
    public required string Description { get; init; }
    /// <summary>
    /// Whether this is an internal module or not.
    /// </summary>
    public bool IsInternal { get; init; }
}
