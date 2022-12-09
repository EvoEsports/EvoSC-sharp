namespace EvoSC.Modules.Interfaces;

/// <summary>
/// Allows the plugin to provide custom install and uninstall code.
/// </summary>
public interface IInstallable
{
    /// <summary>
    /// Install the module.
    /// </summary>
    /// <returns></returns>
    public Task Install();
    
    /// <summary>
    /// Uninstall the modules
    /// </summary>
    /// <returns></returns>
    public Task Uninstall();
}
