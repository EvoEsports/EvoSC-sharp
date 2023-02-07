using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Util.MatchSettings;

/// <summary>
/// List of default modes in Trackmania.
/// </summary>
public enum DefaultModeScriptName
{
    [Identifier(Name = "Trackmania/TM_TimeAttack_Online.Script.txt", NoPrefix = true)]
    [Alias(Name = "ta")]
    TimeAttack,
    
    [Identifier(Name = "Trackmania/TM_Rounds_Online.Script.txt", NoPrefix = true)]
    Rounds,
    
    [Identifier(Name = "Trackmania/TM_Cup_Online.Script.txt", NoPrefix = true)]
    Cup,
    
    [Identifier(Name = "Trackmania/TM_Teams_Online.Script.txt", NoPrefix = true)]
    Teams,
    
    [Identifier(Name = "Trackmania/TM_Laps_Online.Script.txt", NoPrefix = true)]
    Laps,
    
    [Identifier(Name = "Trackmania/TM_Knockout_Online.Script.txt", NoPrefix = true)]
    Knockout,

    [Identifier(Name = "Trackmania/TM_TMWTTeams_Online.Script.txt", NoPrefix = true)]
    TmwtTeams,
    
    /// <summary>
    /// Typically indicates a custom game mode.
    /// </summary>
    Unknown
}
