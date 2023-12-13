namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class ModuleAuthorAttribute(string author) : Attribute
{
    /// <summary>
    /// The module's author.
    /// </summary>
    public string Author { get; } = author;
}
