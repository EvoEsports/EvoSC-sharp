using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchReadyModule.Interfaces;

public interface IReadyTrackerService
{
    /// <summary>
    /// List of players that are ready.
    /// </summary>
    public IEnumerable<IPlayer> ReadyPlayers { get; }
    
    /// <summary>
    /// Players that are tracked for the ready status.
    /// </summary>
    public IEnumerable<IPlayer> Players { get; }
    
    /// <summary>
    /// Whether all players tracked are ready.
    /// </summary>
    public bool AllReady { get; }
    
    /// <summary>
    /// Whether the ready tracker is enabled or not.
    /// </summary>
    public bool Enabled { get; }
    
    /// <summary>
    /// Add a player to the tracker.
    /// </summary>
    /// <param name="player">The player to add.</param>
    /// <returns></returns>
    public Task AddPlayerAsync(IPlayer player);
    
    /// <summary>
    /// Remove a player from the tracker.
    /// </summary>
    /// <param name="player">The player to remove.</param>
    /// <returns></returns>
    public Task RemovePlayerAsync(IPlayer player);
    
    /// <summary>
    /// Set a player as ready.
    /// </summary>
    /// <param name="player">The player to set as ready.</param>
    /// <returns></returns>
    public Task AddReadyAsync(IPlayer player);
    
    /// <summary>
    /// Remove the ready status of a player.
    /// </summary>
    /// <param name="player">The player to set as unready.</param>
    /// <returns></returns>
    public Task RemoveReadyAsync(IPlayer player);
    
    /// <summary>
    /// Clear the entire state of the tracker to the initial state.
    /// </summary>
    /// <returns></returns>
    public Task ClearAsync();
    
    /// <summary>
    /// Enable the tracker.
    /// </summary>
    /// <returns></returns>
    public Task EnableAsync();
    
    /// <summary>
    /// Disable the tracker.
    /// </summary>
    /// <returns></returns>
    public Task DisableAsync();
}
