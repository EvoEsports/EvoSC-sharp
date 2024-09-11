using EvoSC.Modules.Official.GameModeUiModule.Models;

namespace EvoSC.Modules.Official.GameModeUiModule.Interfaces;

public interface IGameModeUiModuleService
{
    public Task InitializeAsync();
    
    /// <summary>
    /// Applies the configured UI modules property values.
    /// </summary>
    /// <param name="componentSettingsList"></param>
    /// <returns></returns>
    public Task ApplyConfigurationAsync(List<GameModeUiComponentSettings> componentSettingsList);

    public Task ApplyComponentSettingsAsync(GameModeUiComponentSettings componentSettings);
    public Task ApplyAndSaveComponentSettingsAsync(GameModeUiComponentSettings componentSettings);

    /// <summary>
    /// Returns the configured UI modules properties as JSON string.
    /// </summary>
    /// <returns></returns>
    public string GetUiModulesPropertiesJson(List<GameModeUiComponentSettings> componentSettingsList);

    public List<GameModeUiComponentSettings> GetDefaultSettings();

    /// <summary>
    /// Generates a UI properties object for the given values.
    /// </summary>
    /// <param name="componentSettings"></param>
    /// <returns></returns>
    public dynamic GeneratePropertyObject(GameModeUiComponentSettings componentSettings);
}
