using EvoSC.Common.Interfaces.Models.Enums;

namespace EvoSC.Common.Interfaces.Models;

/// <summary>
/// Represents a player in-game on the server.
/// </summary>
public interface IOnlinePlayer : IPlayer
{
    /// <summary>
    /// The current in-game state of the player.
    /// </summary>
    public PlayerState State { get; }
    
    /// <summary>
    /// Flags assigned to the player.
    /// </summary>
    public IPlayerFlags Flags { get; }
    
    /// <summary>
    /// The team the player is part of.
    /// </summary>
    public PlayerTeam Team { get; }
}
