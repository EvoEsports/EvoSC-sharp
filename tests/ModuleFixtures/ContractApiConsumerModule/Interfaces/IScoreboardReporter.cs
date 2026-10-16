using EvoSC.Modules.Official.ContractApiProviderModule.Contracts;

namespace EvoSC.Modules.Official.ContractApiConsumerModule.Interfaces;

public interface IScoreboardReporter
{
    Task<MatchResult> ReportAsync(string matchId, string blue, string red, int blueScore, int redScore);

    MatchResult? CurrentResult();
}