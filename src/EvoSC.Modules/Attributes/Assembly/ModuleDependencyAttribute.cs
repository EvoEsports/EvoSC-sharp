namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class ModuleDependencyAttribute : Attribute
{
    /// <summary>
    /// The identifier name of the dependency.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// The version required for this dependency.
    /// </summary>
    public string RequiredVersion { get; }

    public ModuleDependencyAttribute(string name, string requiredVersion)
    {
        Name = name;
        RequiredVersion = requiredVersion;
    }
}
