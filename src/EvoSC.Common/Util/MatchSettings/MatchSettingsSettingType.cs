using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Util.MatchSettings;

/// <summary>
/// A list of possible types for match settings.
/// </summary>
public enum MatchSettingsSettingType
{
    /// <summary>
    /// Any type of text.
    /// </summary>
    [Identifier(Name = "text", NoPrefix = true)]
    String,
    
    /// <summary>
    /// Any number.
    /// </summary>
    [Identifier(Name = "integer", NoPrefix = true)]
    Integer,
    
    /// <summary>
    /// Any boolean value.
    /// </summary>
    [Identifier(Name = "boolean", NoPrefix = true)]
    Boolean,
    
    /// <summary>
    /// A float/real value.
    /// </summary>
    [Identifier(Name = "real", NoPrefix = true)]
    Float
}
