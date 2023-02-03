using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Util.MatchSettings;

public enum MatchSettingsSettingType
{
    [Identifier(Name = "string", NoPrefix = true)]
    String,
    
    [Identifier(Name = "integer", NoPrefix = true)]
    Integer,
    
    [Identifier(Name = "boolean", NoPrefix = true)]
    Boolean
}
