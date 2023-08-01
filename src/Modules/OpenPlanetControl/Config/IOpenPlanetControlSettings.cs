using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.OpenPlanetControl.Config;

[Settings]
public interface IOpenPlanetControlSettings
{
    [Option(DefaultValue = "COMPETITION REGULAR OFFICIAL"), Description("Allowed signature types: REGULAR, DEVMODE, OFFICIAL, COMPETITION")]
    public string[] AllowedTypes { get; set; }
    
    [Option(DefaultValue = 30), Description("Time to wait before kicking the player.")]
    public int KickTimeout { get; set; }

    [Option(DefaultValue = true), Description("Enable continous check of the signature mode of the player while they are on the server.")]
    public bool ContinousChecksEnabled { get; set; }
    
    [Option(DefaultValue = 5000),Description("Number of milliseconds to wait between the continous checks.")]
    public int CheckInterval { get; set; }
    
    [Option(DefaultValue = true), Description("Enable checking of the signature mode of a player when they join.")]
    public bool SignatureModeCheckEnabled { get; set; }
}
