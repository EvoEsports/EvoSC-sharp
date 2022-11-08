namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ModuleAttribute : Attribute
{
    public string Name { get; init; }
    public string Description { get; init; }
    public bool IsInternal { get; init; }
    
    public ModuleAttribute(string name="", string description="")
    {
        Name = name;
        Description = description;
        IsInternal = false;
    }
}
