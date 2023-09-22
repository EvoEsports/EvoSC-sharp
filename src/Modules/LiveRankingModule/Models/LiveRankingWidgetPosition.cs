using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.LiveRankingModule.Models;

public record LiveRankingWidgetPosition(int position, IPlayer player, string login, string time, int cpIndex, bool isFinish);
