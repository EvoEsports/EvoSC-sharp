using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.LiveRankingModule.Models;

public record LiveRankingWidgetPosition(int Position, IPlayer Player, string Login, string Time, int CpIndex, bool IsFinish);
