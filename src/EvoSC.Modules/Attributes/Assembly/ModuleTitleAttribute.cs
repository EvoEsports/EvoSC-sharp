namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class ModuleTitleAttribute : Attribute
{
    /// <summary>
    /// The title of the module.
    /// </summary>
    public string Title { get; }

    public ModuleTitleAttribute(string title)
    {
        Title = title;
    }
}
