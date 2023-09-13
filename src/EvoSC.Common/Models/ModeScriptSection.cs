using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Models;

public enum ModeScriptSection
{
    [Identifier(Name = "", NoPrefix = true)]
    Undefined,
    
    /// <summary>
    /// Triggered before the EndRound section, typically when the finish timeout has ended.
    /// </summary>
    [Identifier(NoPrefix = true)]
    PreEndRound,
    
    /// <summary>
    /// When the current round has finished and scores are determined.
    /// </summary>
    [Identifier(NoPrefix = true)]
    EndRound,

    /// <summary>
    /// When the current map has ended, typically on map change.
    /// </summary>
    [Identifier(NoPrefix = true)]
    EndMap,
    
    /// <summary>
    /// Before the match has ended, typically when the podium begins.
    /// </summary>
    [Identifier(NoPrefix = true)]
    EndMatchEarly,
    
    /// <summary>
    /// When the match has ended, typically on map change.
    /// </summary>
    [Identifier(NoPrefix = true)]
    EndMatch
}
