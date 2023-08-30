using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Evo.GeardownModule.Settings;

[Settings]
public interface IGeardownSettings
{
    [Option(DefaultValue = false)] 
    public bool MatchBegin { get; set; }
    
    [Option, Description("The base URL for the geardown API.")]
    public string ApiBaseUrl { get; set; }
    
    [Option, Description("The access token for the geardown API.")]
    public string ApiAccessToken { get; set; }
    
    [Option, Description("List of player's account IDs which are always whitelisted for matches.")]
    public string Whitelist { get; }
}
