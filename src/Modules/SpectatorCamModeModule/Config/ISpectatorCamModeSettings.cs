using System.ComponentModel;
using Config.Net;
using EvoSC.Common.Util.Manialinks;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.SpectatorCamModeModule.Config;

[Settings]
public interface ISpectatorCamModeSettings
{
    [Option(DefaultValue = "right"),
     Description("Specifies the alignment of the widget, allowed values are: left, right and center.")]
    public WidgetPosition Alignment { get; set; }

    [Option(DefaultValue = 158.0), Description("Defines the horizontal position of the widget.")]
    public double X { get; set; }

    [Option(DefaultValue = -82.5), Description("Defines the vertical position of the widget.")]
    public double Y { get; set; }
}
