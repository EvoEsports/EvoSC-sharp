using EvoSC.Common.Remote.EventArgsModels;

namespace SpectatorTargetInfo.Interfaces;

public interface ISpectatorTargetInfoService
{
    /// <summary>
    /// Sends the manialink to all players.
    /// </summary>
    public Task SendManiaLinkAsync();
    
    /// <summary>
    /// Sends the manialink to a specific player.
    /// </summary>
    public Task SendManiaLinkAsync(string playerLogin);
    
    /// <summary>
    /// Hides the manialink for all players.
    /// </summary>
    public Task HideManiaLinkAsync();
    
    /// <summary>
    /// Hides the default spectator info.
    /// </summary>
    public Task HideNadeoSpectatorInfoAsync();
    
    /// <summary>
    /// Shows the default spectator info.
    /// </summary>
    public Task ShowNadeoSpectatorInfoAsync();
    
    /// <summary>
    /// Maps wayPointEventArgs and sends data to clients.
    /// </summary>
    public Task ForwardCheckpointTimeToClientsAsync(WayPointEventArgs wayPointEventArgs);
    
    /// <summary>
    /// Clears the checkpoint times for the clients.
    /// </summary>
    public Task ResetCheckpointTimesAsync();
    
    /// <summary>
    /// Sends players DNF to clients.
    /// </summary>
    public Task ForwardDnfToClientsAsync(PlayerUpdateEventArgs playerUpdateEventArgs);
}
