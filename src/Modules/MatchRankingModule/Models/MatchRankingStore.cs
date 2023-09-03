using EvoSC.Common.Remote.EventArgsModels;

namespace EvoSC.Modules.Official.MatchRankingModule.Models;

internal class MatchRankingStore
{
    private List<ScoresEventArgs> _scoresStore = new();

    internal Task ConsumeScores(ScoresEventArgs scores)
    {
        _scoresStore.Add(scores);

        return Task.CompletedTask;
    }

    public ScoresEventArgs? GetLatestMatchScores()
    {
        return _scoresStore.LastOrDefault();
    }

    public ScoresEventArgs? GetPreviousMatchScores()
    {
        return _scoresStore.Count < 2 ? null : _scoresStore[^2];
    }

    public Task ResetScores()
    {
        _scoresStore = new List<ScoresEventArgs>();

        return Task.CompletedTask;
    }
}
