namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class ModuleVersionAttribute : Attribute
{
    public Version Version { get; }

    public ModuleVersionAttribute(string version)
    {
        this.Version = Version.Parse(version);
    }
}
