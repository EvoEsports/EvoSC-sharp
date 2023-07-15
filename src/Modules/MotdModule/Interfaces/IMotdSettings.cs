using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.MotdModule.Interfaces;

[Settings]
public interface IMotdSettings
{
    [Option(DefaultValue = "https://directus.evoesports.de/items/motd?filter[server][_eq]=testserver")]
    public string MotdUrl { get; set; }
    
    [Option(DefaultValue = 600000)]
    public int MotdFetchInterval { get; set; }
}
