namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class ModuleVersionAttribute(string version) : Attribute
{
    /// <summary>
    /// The current version of the module.
    /// </summary>
    public Version Version { get; } = Version.Parse((string)version);
}
