using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.ConfigManialinkModule.Config;

[Settings]
public interface IConfigManialinkSettings
{
    [Option(DefaultValue = "greeting"), Description("The text rendered by the module's template.")]
    public string Greeting { get; set; }

    [Option(DefaultValue = 42), Description("A number rendered by the module's template.")]
    public int Answer { get; set; }
}
