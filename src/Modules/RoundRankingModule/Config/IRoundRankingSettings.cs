using System.ComponentModel;
using Config.Net;
using EvoSC.Common.Util.Manialinks;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.RoundRankingModule.Config;

[Settings]
public interface IRoundRankingSettings
{
    [Option(DefaultValue = WidgetPosition.Left)]
    [Description("Specifies on which side the widget is displayed. Valid values are Left | Right.")]
    public WidgetPosition Position { get; set; }

    [Option(DefaultValue = 15.0), Description("Defines the Y position of the widget.")]
    public double Y { get; set; }

    [Option(DefaultValue = 8), Description("Limits the rows shown in the widget.")]
    public int MaxRows { get; set; }

    [Option(DefaultValue = false),
     Description("Shows the time difference to the leading player instead of individual times.")]
    public bool DisplayTimeDifference { get; set; }

    [Option(DefaultValue = true),
     Description("Shows the gained points once a player crosses the finish line.")]
    public bool DisplayGainedPoints { get; set; }
}
