using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.ScoreboardModule.Config;

[Settings]
public interface IScoreboardSettings
{
    [Option(DefaultValue = 160.0), Description("Sets the width of the scoreboard.")]
    public double Width { get; }

    [Option(DefaultValue = 80.0), Description("Sets the height of the scoreboard.")]
    public double Height { get; }
}
