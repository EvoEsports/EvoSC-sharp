namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class ModuleSummaryAttribute : Attribute
{
    /// <summary>
    /// A short description of the module.
    /// </summary>
    public string Summary { get; }

    public ModuleSummaryAttribute(string summary)
    {
        Summary = summary;
    }
}
