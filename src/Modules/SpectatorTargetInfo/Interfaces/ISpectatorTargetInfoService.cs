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
}
