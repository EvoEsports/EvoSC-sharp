namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class InternalModuleAttribute : ModuleAttribute
{
    public InternalModuleAttribute(string name, string description) : base(name, description)
    {
        IsInternal = true;
    }
}
