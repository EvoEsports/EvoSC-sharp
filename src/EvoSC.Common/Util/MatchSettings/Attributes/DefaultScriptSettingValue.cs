namespace EvoSC.Common.Util.MatchSettings.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class DefaultScriptSettingValue : Attribute
{
    /// <summary>
    /// The mode which this value applies to.
    /// </summary>
    public DefaultModeScriptName OnMode { get; set; } = DefaultModeScriptName.Unknown;
    
    /// <summary>
    /// The default value.
    /// </summary>
    public object? Value { get; set; }
}
