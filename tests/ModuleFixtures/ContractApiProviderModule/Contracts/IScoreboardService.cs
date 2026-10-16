using EvoSC.Modules;

namespace EvoSC.Modules.Official.ContractApiProviderModule.Contracts;

/// <summary>
/// Cross-module API for scoreboard management. Marked [Export] so it (and every type
/// referenced by its methods and properties, such as <see cref="MatchResult"/>) is
/// emitted into the module's export assembly and shared with consuming modules under a
/// single type identity.
/// </summary>
[Export]
public interface IScoreboardService
{
    Task<MatchResult> PostScoreAsync(MatchResult result);

    MatchResult? GetCurrentResult();
}