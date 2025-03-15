using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Official.OpenPlanetModule.Models;

namespace EvoSC.Modules.Official.OpenPlanetModule.Config;

[Settings]
public interface IOpenPlanetControlSettings
{
    [Option(DefaultValue = OpenPlanetSignatureMode.Regular 
                           | OpenPlanetSignatureMode.TMGL 
                           | OpenPlanetSignatureMode.Official)]
    [Description("Allowed signature types: Regular, DevMode, Official, Competition")]
    public OpenPlanetSignatureMode AllowedSignatureModes { get; set; }

    [Option(DefaultValue = 30), Description("Time to wait before kicking the player.")]
    public int KickTimeout { get; set; }

    [Option(DefaultValue = false), Description("Enable continous check of the signature mode of the player while they are on the server.")]
    public bool ContinuousChecksEnabled { get; set; }
    
    [Option(DefaultValue = 5000),Description("Number of milliseconds to wait between the continous checks.")]
    public int CheckInterval { get; set; }
    
    [Option(DefaultValue = true), Description("Enable checking of the signature mode of a player when they join.")]
    public bool SignatureModeCheckEnabled { get; set; }
    
    [Option(DefaultValue = true), Description("Allow the use of openplanet. If false, no signature mode is allowed.")]
    public bool AllowOpenplanet { get; set; }
    
    [Option(DefaultValue = "1.26.25"), Description("The minimum required OpenPlanet version to play on this server.")]
    public Version MinimumRequiredVersion { get; set; }
    
    [Option(DefaultValue = true), Description("Enable auditing of all checks.")]
    public bool AuditAllChecks { get; set; }
    
    [Option(DefaultValue = true), Description("Enable auditing of players that got jailed.")]
    public bool AuditJails { get; set; }
}
