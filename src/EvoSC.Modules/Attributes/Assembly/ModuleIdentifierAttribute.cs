namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class ModuleIdentifierAttribute(string name) : Attribute
{
    /// <summary>
    /// The unique identifier of the module.
    /// </summary>
    public string Name { get; } = name;
}
