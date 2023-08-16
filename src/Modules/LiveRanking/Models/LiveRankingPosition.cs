namespace EvoSC.Modules.Official.LiveRankingModule.Models;

public record LiveRankingPosition(string accountId, int cpTime, int cpIndex, bool isDNF, bool isFinish);
