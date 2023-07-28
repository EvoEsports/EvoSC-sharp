using System.ComponentModel;
using Config.Net;

namespace EvoSC.Common.Config.Models;

public interface IPathConfig
{
    [Description("Path to the maps folder")]
    [Option(Alias = "maps")]
    public string Maps { get; }
    
    [Description("Default match settings file")]
    [Option(Alias = "defaultMatchSettings")]
    public string DefaultMatchSettings { get; }
}
