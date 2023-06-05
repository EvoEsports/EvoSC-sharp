using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Remote;

public enum ModeScriptEvent
{
    /// <summary>
    /// Teams and player scores reported.
    /// </summary>
    [Identifier(Name = "ModeScript.Scores")]
    Scores,
    
    /// <summary>
    /// When a player gives up and restart from the beginning.
    /// </summary>
    [Identifier(Name = "ModeScript.GiveUp")]
    GiveUp,
    
    /// <summary>
    /// When a player crosses a checkpoint.
    /// </summary>
    [Identifier(Name = "ModeScript.WayPoint")]
    WayPoint,
    
    /// <summary>
    /// When a player restarts back to the previous checkpoint.
    /// </summary>
    [Identifier(Name = "ModeScript.Respawn")]
    Respawn,

    /// <summary>
    /// When a player starts the race at end of the countdown sequence.
    /// </summary>
    [Identifier(Name = "ModeScript.StartLine")]
    StartLine,
    
    /// <summary>
    /// When the warm-up ends.
    /// </summary>
    [Identifier(Name = "ModeScript.WarmUpEnd")]
    WarmUpEnd,
    
    /// <summary>
    /// When the warm-up starts.
    /// </summary>
    [Identifier(Name = "ModeScript.WarmUpStart")]
    WarmUpStart,
    
    /// <summary>
    /// When a player got eliminated by an obstacle.
    /// </summary>
    [Identifier(Name = "ModeScript.Eliminated")]
    Eliminated,

    /// <summary>
    /// When a player got eliminated by an obstacle.
    /// </summary>
    [Identifier(Name = "ModeScript.PodiumStart")]
    PodiumStart,
    
    /// <summary>
    /// When a player got eliminated by an obstacle.
    /// </summary>
    [Identifier(Name = "ModeScript.PodiumEnd")]
    PodiumEnd,
    
    /// <summary>
    /// When any Mode Script callback was called.
    /// </summary>
    [Identifier(Name = "ModeScript.Any")]
    Any
}
