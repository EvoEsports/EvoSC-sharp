using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Interfaces.Models;

public enum PlayerTeam
{
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
