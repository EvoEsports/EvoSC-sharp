using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.OpenPlanetControl.Models;

public enum OpenPlanetSignatureMode
{
    /// <summary>
    /// Openplanet is not detected or the signature mode is unrecognizable.
    /// </summary>
    [Identifier(Name = "UNKNOWN", NoPrefix = true)]
    Unknown = 0,
    
    /// <summary>
    /// All signed plugins.
    /// </summary>
    [Identifier(Name = "REGULAR", NoPrefix = true)]
    Regular = 1,
    
    /// <summary>
    /// All signed and unsigned plugins.
    /// </summary>
    [Identifier(Name = "DEVMODE", NoPrefix = true)]
    DevMode = 2 ,
    
    /// <summary>
    /// Only plugins shipped with OpenPlanet.
    /// </summary>
    [Identifier(Name = "OFFICIAL", NoPrefix = true)]
    Official = 4,
    
    /// <summary>
    /// Only plugins approved for use in TMGL.
    /// </summary>
    [Identifier(Name = "COMPETITION", NoPrefix = true)]
    Competition = 8
}
