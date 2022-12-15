namespace EvoSC.Modules.Attributes;

/// <summary>
/// Defines any dependencies this module requires.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RequireDependencyAttribute : Attribute
{
    /// <summary>
    /// Name of the module that is dependent upon.
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// A specific version of the dependency that is required.
    /// </summary>
    public Version? Version { get; set; }
}
