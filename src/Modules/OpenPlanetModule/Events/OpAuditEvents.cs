using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.OpenPlanetModule.Events;

public enum OpAuditEvents
{
    [Identifier(Name = "OpenPlanet:SignatureModeCheck")]
    SignatureModeCheck,
    
    [Identifier(Name = "OpenPlanet:PlayerJailed")]
    PlayerJailed
}
