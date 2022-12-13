namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class ModuleAuthorAttribute : Attribute
{
    /// <summary>
    /// The module's author.
    /// </summary>
    public string Author { get; }

    public ModuleAuthorAttribute(string author)
    {
        Author = author;
    }
}
