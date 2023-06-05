namespace EvoSC.Modules.Official.EvoSCTemplateModule.Interfaces;

public interface IEvoSCTemplateService
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
