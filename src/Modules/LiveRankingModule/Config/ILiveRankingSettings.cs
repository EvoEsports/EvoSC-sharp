using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.LiveRankingModule.Config;

[Settings]
public interface ILiveRankingSettings
{
    [Option(DefaultValue = 10), Description("Max of rows to show in the live ranking widget.")]
    public int MaxWidgetRows { get; set; }
    
    [Option(DefaultValue = 63.0), Description("Specifies the Y position of the widget.")]
    public double Y { get; set; }
    
    [Option(DefaultValue = 36.0), Description("Specifies the width of the widget.")]
    public double Width { get; set; }
    
    [Option(DefaultValue = "right"), Description("Specifies on which side the widget is displayed.")]
    public string Position { get; set; }
}
