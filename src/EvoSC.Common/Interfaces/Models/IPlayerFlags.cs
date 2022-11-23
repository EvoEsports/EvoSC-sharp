namespace EvoSC.Common.Interfaces.Models;

public interface IPlayerFlags
{
    /// <summary>
    /// Whether the player is forced into spectator mode.
    /// </summary>
    public bool ForceSpectator { get; }
    
    /// <summary>
    /// Whether the player is forced into spectator mode,
    /// but spectator mode is still selectable for them.
    /// </summary>
    public bool ForceSpectatorSelectable { get; }
    
    /// <summary>
    /// The stereo display mode.
    /// </summary>
    public int StereoDisplayMode { get; }
    
    /// <summary>
    /// Whether the player is managed by another server.
    /// </summary>
    public bool IsManagedByAnOtherServer { get; }
    
    /// <summary>
    /// Whether the player is the server itself.
    /// </summary>
    public bool IsServer { get; }
    
    /// <summary>
    /// Whether the player has a slot for playing on the server.
    /// </summary>
    public bool HasPlayerSlot { get; }
    
    /// <summary>
    /// Whether the player is broadcasting.
    /// </summary>
    public bool IsBroadcasting { get; }
    
    /// <summary>
    /// Whether the player has joined the game or not yet.
    /// </summary>
    public bool HasJoinedGame { get; }
}
