namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class ModuleDependencyAttribute : Attribute
{
    public string Name { get; }
    public string RequiredVersion { get; }

    public ModuleDependencyAttribute(string name, string requiredVersion)
    {
        Name = name;
        RequiredVersion = requiredVersion;
    }
}
