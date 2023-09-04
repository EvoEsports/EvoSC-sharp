using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.LiveRankingModule.Models;

public record LiveRankingWidgetPosition(int position, IPlayer player, string time);
