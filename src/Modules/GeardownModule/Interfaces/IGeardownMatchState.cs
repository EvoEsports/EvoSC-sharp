using EvoSC.Modules.Evo.GeardownModule.Models.API;

namespace EvoSC.Modules.Evo.GeardownModule.Interfaces;

public interface IGeardownMatchState
{
    /// <summary>
    /// Details about the match.
    /// </summary>
    public GdMatch Match { get; }
    
    /// <summary>
    /// The match token for this match.
    /// </summary>
    public string MatchToken { get; }
}
