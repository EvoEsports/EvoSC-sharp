namespace EvoSC.Modules.Official.OpenPlanetModule.Models;

public enum OpJailReason
{
    /// <summary>
    /// The player is using a version lower than the minimum required for the server.
    /// </summary>
    InvalidVersion,
    
    /// <summary>
    /// The player is using a prohibited signature mode for the server.
    /// </summary>
    InvalidSignatureMode,
    
    /// <summary>
    /// The player is using OpenPlanet while the server does not allow it.
    /// </summary>
    OpenPlanetNotAllowed
}
