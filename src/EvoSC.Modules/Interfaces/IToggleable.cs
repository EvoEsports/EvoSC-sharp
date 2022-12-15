namespace EvoSC.Modules;

/// <summary>
/// Defines a module as toggleable with custom enable/disable methods.
/// </summary>
public interface IToggleable
{
    /// <summary>
    /// Enable the module.
    /// </summary>
    /// <returns></returns>
    public Task Enable();
    
    /// <summary>
    /// Disable the module.
    /// </summary>
    /// <returns></returns>
    public Task Disable();
}
