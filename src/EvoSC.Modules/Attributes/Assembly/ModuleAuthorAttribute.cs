namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class ModuleAuthorAttribute : Attribute
{
    public string Author { get; }

    public ModuleAuthorAttribute(string author)
    {
        Author = author;
    }
}
