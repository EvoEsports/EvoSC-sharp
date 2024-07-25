using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Interfaces.Models;

public enum PlayerTeam
{
    /// <summary>
    /// Player is in an unknown team, for example
    /// in a mode that doesn't support teams.
    /// </summary>
    [Identifier(Name = "-1")]
    Unknown = -1,
    
    /// <summary>
    /// The first/blue/left team.
    /// </summary>
    [Identifier(Name = "0")]
    Team1 = 0,
    
    /// <summary>
    /// The second/red/right team.
    /// </summary>
    [Identifier(Name = "1")]
    Team2 = 1
}
