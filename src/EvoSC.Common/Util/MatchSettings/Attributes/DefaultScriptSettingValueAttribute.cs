using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Util.MatchSettings.Attributes;

/// <summary>
/// Set a default value for a script setting.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class DefaultScriptSettingValueAttribute : Attribute
{
    /// <summary>
    /// The mode which this value applies to. If null,
    /// applies to all modes.
    /// </summary>
    public string? OnMode { get; set; }
    
    /// <summary>
    /// The default value.
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// Set the default value on one of the default modes.
    /// </summary>
    /// <param name="onMode">The mode to set the value to.</param>
    /// <param name="value">The default value.</param>
    public DefaultScriptSettingValueAttribute(DefaultModeScriptName onMode, object? value)
    {
        OnMode = onMode.GetIdentifier();
        Value = value;
    }
    
    /// <summary>
    /// Set the default value for a custom mode.
    /// </summary>
    /// <param name="onMode">The name of the mode to set the value to.</param>
    /// <param name="value">The default value.</param>
    public DefaultScriptSettingValueAttribute(string onMode, object? value)
    {
        OnMode = onMode;
        Value = value;
    }

    /// <summary>
    /// Set the default value for all modes.
    /// </summary>
    /// <param name="value">The default value.</param>
    public DefaultScriptSettingValueAttribute(object? value)
    {
        OnMode = null;
        Value = value;
    }
}
