using EvoSC.Modules.Official.GameModeUiModule.Models;

namespace EvoSC.Modules.Official.SpectatorCamModeModule.Interfaces;

public interface ISpectatorCamModeService
{
    /// <summary>
    /// Sends a persistent cam mode selection widget.
    /// </summary>
    /// <returns></returns>
    public Task SendPersistentCamModeWidgetAsync();
    
    /// <summary>
    /// Hides the default UI provided by the game mode.
    /// </summary>
    /// <returns></returns>
    public Task HideGameModeUiAsync();

    /// <summary>
    /// Gets the settings to hide the default UI component.
    /// </summary>
    /// <returns></returns>
    public GameModeUiComponentSettings GetGameModeUiSettings();
}
