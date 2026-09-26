using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.ContractApiProviderModule.Contracts;

namespace EvoSC.Modules.Official.ContractApiProviderModule.Services;

/// <summary>
/// Default implementation of the shared <see cref="IScoreboardService"/> export.
/// </summary>
[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class ScoreboardService : IScoreboardService
{
    private volatile MatchResult? _current;

    public Task<MatchResult> PostScoreAsync(MatchResult result)
    {
        _current = result;
        return Task.FromResult(result);
    }

    public MatchResult? GetCurrentResult() => _current;
}