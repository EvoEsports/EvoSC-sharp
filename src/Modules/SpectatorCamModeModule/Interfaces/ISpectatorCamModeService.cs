namespace EvoSC.Modules.Official.SpectatorCamModeModule.Interfaces;

public interface ISpectatorCamModeService
{
    public Task SendCamModeWidgetAsync();

    public Task HideCamModeWidgetAsync();
    
    public Task HideGameModeUiAsync();
}
