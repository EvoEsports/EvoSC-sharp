namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class ModuleIdentifierAttribute : Attribute
{
    public string Name { get; }

    public ModuleIdentifierAttribute(string name)
    {
        Name = name;
    }
}
