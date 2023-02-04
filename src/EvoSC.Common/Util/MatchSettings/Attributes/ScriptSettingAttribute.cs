namespace EvoSC.Common.Util.MatchSettings.Attributes;

/// <summary>
/// Defines a property as a script setting.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ScriptSettingAttribute : Attribute
{
    /// <summary>
    /// The name of the script setting.
    /// </summary>
    public string Name { get; set; }
}
