namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class ModuleDependencyAttribute(string name, string requiredVersion) : Attribute
{
    /// <summary>
    /// The identifier name of the dependency.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// The version required for this dependency.
    /// </summary>
    public string RequiredVersion { get; } = requiredVersion;
}
