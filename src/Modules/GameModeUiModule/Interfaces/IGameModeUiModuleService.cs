using EvoSC.Modules.Official.GameModeUiModule.Models;

namespace EvoSC.Modules.Official.GameModeUiModule.Interfaces;

/// <summary>
/// Exported so other modules can drive the game mode UI without a reference to this module.
/// GameModeUiComponentSettings and the models it mentions are part of the export surface too,
/// so both sides exchange the same types.
/// </summary>
[Export]
public interface IGameModeUiModuleService
{
    /// <summary>
    /// Applies the given game mode component settings collection.
    /// </summary>
    /// <param name="componentSettingsList"></param>
    /// <returns></returns>
    public Task ApplyComponentSettingsAsync(IEnumerable<GameModeUiComponentSettings> componentSettingsList);

    /// <summary>
    /// Applies the given game mode component settings.
    /// </summary>
    /// <param name="componentSettings"></param>
    /// <returns></returns>
    public Task ApplyComponentSettingsAsync(GameModeUiComponentSettings componentSettings);

    /// <summary>
    /// Applies the given game mode component settings.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="visible"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public Task ApplyComponentSettingsAsync(string name, bool visible, double x, double y, double scale);

    /// <summary>
    /// Returns the configured UI modules properties as JSON string.
    /// </summary>
    /// <returns></returns>
    public string GetUiModulesPropertiesJson(IEnumerable<GameModeUiComponentSettings> componentSettingsList);

    /// <summary>
    /// Generates a UI properties object for the given values.
    /// </summary>
    /// <param name="componentSettings"></param>
    /// <returns></returns>
    public dynamic GeneratePropertyObject(GameModeUiComponentSettings componentSettings);

    /// <summary>
    /// Gets the default game mode component settings collection.
    /// </summary>
    /// <returns></returns>
    public List<GameModeUiComponentSettings> GetDefaultSettings();
}
