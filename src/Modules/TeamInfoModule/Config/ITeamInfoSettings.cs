using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.TeamInfoModule.Config;

[Settings]
public interface ITeamInfoSettings
{
    [Option(DefaultValue = 0.0), Description("Specifies the horizontal center position of the widget.")]
    public double X { get; set; }

    [Option(DefaultValue = 80.0), Description("Specifies the vertical top edge position of the wigdet. Values from -90 to 90 allowed.")]
    public double Y { get; set; }
    
    [Option(DefaultValue = 1.0), Description("Specifies the scale of the widget.")]
    public double Scale { get; set; }

    [Option(DefaultValue = false), Description("If enabled a smaller version of the widget is shown, without the large team name.")]
    public bool CompactMode { get; set; }
}
