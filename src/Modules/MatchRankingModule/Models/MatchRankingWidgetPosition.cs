using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchRankingModule.Models;

public record MatchRankingWidgetPosition(int Position, IPlayer Player, string Login, string Time, int CpIndex, bool IsFinish);
