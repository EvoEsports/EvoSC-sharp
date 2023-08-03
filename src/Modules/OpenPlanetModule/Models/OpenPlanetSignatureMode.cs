using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.OpenPlanetModule.Models;

[Flags]
public enum OpenPlanetSignatureMode
{
    /// <summary>
    /// Openplanet is not detected or the signature mode is unrecognizable.
    /// </summary>
    [Identifier(Name = "UNKNOWN", NoPrefix = true)]
    Unknown = 1,
    
    /// <summary>
    /// All signed plugins.
    /// </summary>
    [Identifier(Name = "REGULAR", NoPrefix = true)]
    Regular = 2,
    
    /// <summary>
    /// All signed and unsigned plugins.
    /// </summary>
    [Identifier(Name = "DEVMODE", NoPrefix = true)]
    DevMode = 4,
    
    /// <summary>
    /// Only plugins shipped with OpenPlanet.
    /// </summary>
    [Identifier(Name = "OFFICIAL", NoPrefix = true)]
    Official = 8,
    
    /// <summary>
    /// Only plugins approved for use in TMGL.
    /// </summary>
    [Identifier(Name = "COMPETITION", NoPrefix = true)]
    TMGL = 16
}
