using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.RankingModule.Config;

[Settings]
public interface IRankingSettings
{
    [Option(DefaultValue = 60), Description("Weight of the win rate in the ranking score.")]
    public int WinRateWeight { get; }

    [Option(DefaultValue = 40), Description("Weight of the playtime in the ranking score.")]
    public int PlaytimeWeight { get; }

    [Option(DefaultValue = 100), Description("Hours of playtime that give the full playtime score.")]
    public int PlaytimeCapHours { get; }

    [Option(DefaultValue = 10),
     Description("Win rate is calculated as wins / (maps played + this value), so a few lucky wins do not rank high.")]
    public int WinRateSmoothing { get; }

    [Option(DefaultValue = 100), Description("Number of players to show in the ranking window.")]
    public int TopListSize { get; }
}
