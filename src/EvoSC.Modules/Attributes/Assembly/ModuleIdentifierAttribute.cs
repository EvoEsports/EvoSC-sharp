namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class ModuleIdentifierAttribute : Attribute
{
    /// <summary>
    /// The name of the module.
    /// </summary>
    public string Name { get; }

    public ModuleIdentifierAttribute(string name)
    {
        Name = name;
    }
}
