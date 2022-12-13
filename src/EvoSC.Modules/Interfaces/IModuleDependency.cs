namespace EvoSC.Modules.Interfaces;

public interface IModuleDependency
{
    /// <summary>
    /// The module's unique identifier name.
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// The version required for this module.
    /// </summary>
    public Version Version { get; }
}
