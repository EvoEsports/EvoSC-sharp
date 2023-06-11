namespace EvoSC.Modules.Official.OpenPlanetControl.Interfaces;

public interface IOpenPlanetControlService
{
    /// <summary>
    /// Called on when module is enabled
    /// </summary>
    /// <returns></returns>
    Task onEnable();

    /// <summary>
    /// Called on when module is disabled
    /// </summary>
    /// <returns></returns>
    Task onDisable();
    
    
    
}
