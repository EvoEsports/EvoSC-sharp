using EvoSC.Common.Remote.EventArgsModels;

namespace EvoSC.Modules.Official.MatchRankingModule.Models;

internal class MatchRankingStore
{
    private readonly List<ScoresEventArgs> _scoresStore = new();

    /// <summary>
    /// Add the latest scores to the store.
    /// </summary>
    /// <returns></returns>
    internal Task ConsumeScores(ScoresEventArgs scores)
    {
        _scoresStore.Add(scores);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Get the latest scores entry in the store.
    /// </summary>
    /// <returns></returns>
    public ScoresEventArgs? GetLatestMatchScores()
    {
        return _scoresStore.LastOrDefault();
    }

    /// <summary>
    /// Get the 2nd last scores entry from the store.
    /// </summary>
    /// <returns></returns>
    public ScoresEventArgs? GetPreviousMatchScores()
    {
        return _scoresStore.Count < 2 ? null : _scoresStore[^2];
    }
}
