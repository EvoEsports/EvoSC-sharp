namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class ModuleSummaryAttribute : Attribute
{
    public string Summary { get; }

    public ModuleSummaryAttribute(string summary)
    {
        Summary = summary;
    }
}
