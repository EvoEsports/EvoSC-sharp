using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Models;

public enum StateSubjects
{
    /// <summary>
    /// Subject for published chat messages.
    /// </summary>
    [Identifier(NoPrefix = true)]
    ChatMessages,

    /// <summary>
    /// Subject for published player state updates.
    /// </summary>
    [Identifier(NoPrefix = true)]
    PlayerState,

    [Identifier(NoPrefix = true)]
    MapFinished,

    /// <summary>
    /// Subject for end match state updates.
    /// </summary>
    [Identifier(NoPrefix = true)]
    EndMatch,

    /// <summary>
    /// Subject for end round state updates.
    /// </summary>
    [Identifier(NoPrefix = true)]
    EndRound,

    /// <summary>
    /// Subject for published waypoint state updates.
    /// </summary>
    [Identifier(NoPrefix = true)]
    Waypoint,

    /// <summary>
    /// Subject for published scores state updates.
    /// </summary>
    [Identifier(NoPrefix = true)]
    Scores
}
