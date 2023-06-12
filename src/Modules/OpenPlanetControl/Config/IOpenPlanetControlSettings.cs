using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.OpenPlanetControl.Config;

[Settings]
public interface IOpenPlanetControlSettings
{
    [Option(DefaultValue = "COMPETITION REGULAR"), Description("Allowed signature types: REGULAR, DEVMODE, OFFICIAL, COMPETITION")]
    public string[] AllowedTypes { get; set; }
    
    [Option(DefaultValue = 30), Description("Kick timeout")]
    public int KickTimeout { get; set; }

}
