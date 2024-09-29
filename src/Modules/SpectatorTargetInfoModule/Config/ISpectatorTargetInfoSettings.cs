using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Config;

[Settings]
public interface ISpectatorTargetInfoSettings
{
    [Option(DefaultValue = -57.0), Description("Defines the vertical position of the widget.")]
    public double Y { get; set; }
}
