namespace EvoSC.Modules.Official.UIModule.Interfaces;

public interface IUiModuleService
{
    /// <summary>
    /// Applies the configured UI modules property values.
    /// </summary>
    /// <returns></returns>
    public Task ApplyConfigurationAsync();

    /// <summary>
    /// Returns the configured UI modules properties as JSON string.
    /// </summary>
    /// <returns></returns>
    public Task<string> GetUiModulesPropertiesJsonAsync();

    /// <summary>
    /// Generates a UI properties object for the given values.
    /// </summary>
    /// <param name="uiModuleName"></param>
    /// <param name="visible"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public Task<dynamic> GeneratePropertyObject(string uiModuleName, bool visible, double x, double y, double scale);
}
