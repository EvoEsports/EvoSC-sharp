using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchReadyModule.Interfaces;

public interface IReadyService
{
    /// <summary>
    /// List of players that are ready.
    /// </summary>
    public IEnumerable<IPlayer> ReadyPlayers { get; }
    
    /// <summary>
    /// Players that are selected for playing. They must all be set to ready.
    /// </summary>
    public IEnumerable<IPlayer> Players { get; }
    
    /// <summary>
    /// Whether the ready widget and it's commands are enabled.
    /// </summary>
    public bool Enabled { get; set; }
    
    /// <summary>
    /// Reset everything to the initial state.
    /// </summary>
    /// <returns></returns>
    public Task ResetAsync();
    
    /// <summary>
    /// Add players to the ready widget.
    /// </summary>
    /// <param name="players">Players to add.</param>
    /// <returns></returns>
    public Task AddPlayersAsync(params IPlayer[] players);
    
    /// <summary>
    /// Set the ready status of a single player.
    /// </summary>
    /// <param name="player">The player to set the status for.</param>
    /// <param name="isReady">Whether the player is ready or not.</param>
    /// <returns></returns>
    public Task SetPlayerReadyStatusAsync(IPlayer player, bool isReady);
    
    /// <summary>
    /// Enable the ready widget.
    /// </summary>
    /// <returns></returns>
    public Task EnableAsync();
    
    /// <summary>
    /// Disable the ready widget.
    /// </summary>
    /// <returns></returns>
    public Task DisableAsync();
}
