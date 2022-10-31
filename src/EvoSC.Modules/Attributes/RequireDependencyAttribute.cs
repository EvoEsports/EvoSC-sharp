namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RequireDependencyAttribute : Attribute
{
    public string Name { get; set; }
    public Version? Version { get; set; }

    public RequireDependencyAttribute(string moduleName, Version? version = null)
    {
        Name = moduleName;
        version = version;
    }
}
