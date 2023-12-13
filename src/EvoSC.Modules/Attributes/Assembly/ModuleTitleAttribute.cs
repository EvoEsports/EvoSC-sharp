namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class ModuleTitleAttribute(string title) : Attribute
{
    /// <summary>
    /// The title of the module.
    /// </summary>
    public string Title { get; } = title;
}
