using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.ContractApiConsumerModule.Interfaces;
using EvoSC.Modules.Official.ContractApiProviderModule.Contracts;

namespace EvoSC.Modules.Official.ContractApiConsumerModule.Services;

/// <summary>
/// Consumes the scoreboard API published by the provider module. Resolves through the
/// shared export assembly so the <see cref="IScoreboardService"/> / <see cref="MatchResult"/>
/// types used here are the very same instances the provider registers.
/// </summary>
[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class ScoreboardReporter : IScoreboardReporter
{
    private readonly IScoreboardService _scoreboard;

    public ScoreboardReporter(IScoreboardService scoreboard)
    {
        _scoreboard = scoreboard;
    }

    public Task<MatchResult> ReportAsync(string matchId, string blue, string red, int blueScore, int redScore) =>
        _scoreboard.PostScoreAsync(new MatchResult(matchId, blue, red, blueScore, redScore));

    public MatchResult? CurrentResult() => _scoreboard.GetCurrentResult();
}