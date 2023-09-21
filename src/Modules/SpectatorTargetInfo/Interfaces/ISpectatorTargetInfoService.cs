using EvoSC.Common.Remote.EventArgsModels;

namespace SpectatorTargetInfo.Interfaces;

public interface ISpectatorTargetInfoService
{
    /// <summary>
    /// Sends the manialink to all players.
    /// </summary>
    public Task SendManiaLink();
    /// <summary>
    /// Sends the manialink to a specific player.
    /// </summary>
    public Task SendManiaLink(string playerLogin);
    /// <summary>
    /// Hides the manialink for all players.
    /// </summary>
    public Task HideManiaLink();
    /// <summary>
    /// Hides the default spectator info.
    /// </summary>
    public Task HideNadeoSpectatorInfo();
    /// <summary>
    /// Shows the default spectator info.
    /// </summary>
    public Task ShowNadeoSpectatorInfo();
    /// <summary>
    /// Maps wayPointEventArgs and sends data to clients.
    /// </summary>
    public Task ForwardCheckpointTimeToClients(WayPointEventArgs wayPointEventArgs);
    /// <summary>
    /// Clears the checkpoint times for the clients.
    /// </summary>
    public Task ResetCheckpointTimes();
    /// <summary>
    /// Sends players DNF to clients.
    /// </summary>
    public Task ForwardDnf(PlayerUpdateEventArgs playerUpdateEventArgs);
}
