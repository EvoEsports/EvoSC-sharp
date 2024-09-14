namespace EvoSC.Modules.Official.SpectatorCamModeModule.Interfaces;

public interface ISpectatorCamModeService
{
    public Task InitializeAsync();
    
    public Task SendCamModeWidgetToSpectatorsAsync();
    
    public Task SendCamModeWidgetAsync(string playerLogin);

    public Task HideCamModeWidgetAsync();
    
    public Task HideCamModeWidgetAsync(string playerLogin);
    
    public Task HideGameModeUiAsync();
}
