using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;
using LinqToDB.Common;

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
    
    [Option(DefaultValue = false), Description("Whether to start the match automatically when all players are ready.")]
    public bool AutomaticMatchStart { get; set; }
    
    [Option, Description("The current match state object.")]
    public string MatchState { get; set; }
}
