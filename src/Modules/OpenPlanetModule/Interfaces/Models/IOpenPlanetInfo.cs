using EvoSC.Modules.Official.OpenPlanetModule.Models;

namespace EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;

public interface IOpenPlanetInfo
{
    /// <summary>
    /// The release version of the running OpenPlanet client.
    /// </summary>
    public Version Version { get; set; }
    
    /// <summary>
    /// The name of the game the client is running on.
    /// </summary>
    public string Game { get; set; }
    
    /// <summary>
    /// The release branch of OpenPlanet.
    /// </summary>
    public string Branch { get; set; }
    
    /// <summary>
    /// The build version of OpenPlanet.
    /// </summary>
    public string Build { get; set; }
    
    /// <summary>
    /// Signature mode the user is running OpenPlanet in.
    /// </summary>
    public OpenPlanetSignatureMode SignatureMode { get; set; }
    
    /// <summary>
    /// Whether the user is running OpenPlanet or not.
    /// </summary>
    public bool IsOpenPlanet { get; set; }
}
