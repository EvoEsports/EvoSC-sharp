using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.MotdModule.Interfaces;

[Settings]
public interface IMotdSettings
{
    [Option(DefaultValue = ""), Description("The URL from which the controller fetches the motd.")]
    public string MotdUrl { get; set; }
    
    [Option(DefaultValue = 600000), Description("The interval in which the motd gets fetched from the server.")]
    public int MotdFetchInterval { get; set; }
    
    [Option(DefaultValue = "This is the Motd!"), Description("The locally stored Motd text if it should not be fetched from a server.")]
    public string MotdLocalText { get; set; }
    
    [Option(DefaultValue = true), Description("Indicator if the locally stored motd should be used.")]
    public bool UseLocalMotd { get; set; }
}
