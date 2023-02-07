namespace EvoSC.Common.Util.MatchSettings.Models;

/// <summary>
/// Represent a single mode script setting.
/// </summary>
public class ModeScriptSettingInfo
{
    /// <summary>
    /// The value of the setting.
    /// </summary>
    public object? Value { get; set; }
    
    /// <summary>
    /// Potential description of the setting.
    /// </summary>
    public string Description { get; set; } 
    
    /// <summary>
    /// The data type of the setting.
    /// </summary>
    public Type Type { get; set; }
}
