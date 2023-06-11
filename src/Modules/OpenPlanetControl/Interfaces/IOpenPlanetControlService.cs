namespace EvoSC.Modules.Official.OpenPlanetControl.Interfaces;

public interface IOpenPlanetControlService
{
    /// <summary>
    /// Called on when module is enabled
    /// </summary>
    /// <returns></returns>
    Task OnEnableAsync();

    /// <summary>
    /// Called on when module is disabled
    /// </summary>
    /// <returns></returns>
    Task OnDisableAsync();

    /// <summary>
    /// Called on when player connects and openplanet data is received
    /// </summary>
    /// <param name="login"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    Task OnDetectAsync(string login, string data);

    /// <summary>
    /// Removes cached player info
    /// </summary>
    /// <param name="login"></param>
    void RemovePlayerByLogin(string login);
    
    /// <summary>
    /// Kicks player
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    Task KickAsync(string login);
    
}
