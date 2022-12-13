namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class ModuleVersionAttribute : Attribute
{
    /// <summary>
    /// The current version of the module.
    /// </summary>
    public Version Version { get; }

    public ModuleVersionAttribute(string version)
    {
        this.Version = Version.Parse(version);
    }
}
