namespace EvoSC.Modules.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class ModuleSummaryAttribute(string summary) : Attribute
{
    /// <summary>
    /// A short description of the module.
    /// </summary>
    public string Summary { get; } = summary;
}
