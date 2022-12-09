namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class ModuleTitleAttribute : Attribute
{
    public string Title { get; }

    public ModuleTitleAttribute(string title)
    {
        Title = title;
    }
}
