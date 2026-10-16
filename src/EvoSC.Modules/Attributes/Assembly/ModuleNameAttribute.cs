namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class ModuleNameAttribute(string name) : Attribute
{
    /// <summary>
    /// The display name of the module.
    /// </summary>
    public string Name { get; } = name;
}
