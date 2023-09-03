using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Services;
using EvoSC.Modules.Official.MatchRankingModule.Interfaces;
using EvoSC.Modules.Official.MatchRankingModule.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.MatchRankingModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class MatchRankingService : IMatchRankingService
{
    private readonly IManialinkManager _manialinkManager;
    private readonly IPlayerManagerService _playerManager;
    private readonly ILogger _logger;
    private readonly MatchRankingStore _matchRankingStore;

    public MatchRankingService(IManialinkManager manialinkManager, IPlayerManagerService playerManager, ILogger<LiveRankingService> logger)
    {
        _manialinkManager = manialinkManager;
        _playerManager = playerManager;
        _logger = logger;
        _matchRankingStore = new MatchRankingStore();
    }

    public async Task OnScores(ScoresEventArgs scores)
    {
        await _matchRankingStore.ConsumeScores(scores);
        await SendManialink();
    }

    public async Task SendManialink()
    {
        _logger.LogInformation("Sending manialink");

        var mappedScoresPrevious = MapScoresForWidget(_matchRankingStore.GetPreviousMatchScores());
        var mappedScoresLatest = MapScoresForWidget(_matchRankingStore.GetLatestMatchScores());

        await _manialinkManager.SendManialinkAsync("MatchRankingModule.MatchRanking",
            new { LatestScores = mappedScoresLatest, PreviousScores = mappedScoresPrevious }
        );
    }

    public async Task SendManialink(string playerLogin)
    {
        var player = await _playerManager.GetPlayerAsync(PlayerUtils.ConvertLoginToAccountId(playerLogin));
        var mappedScores = MapScoresForWidget(_matchRankingStore.GetLatestMatchScores());

        await _manialinkManager.SendManialinkAsync(player,
            "MatchRankingModule.MatchRanking",
            new { LatestScores = mappedScores }
        );
    }

    private static IEnumerable<MatchRankingWidgetData> MapScoresForWidget(ScoresEventArgs? scores)
    {
        if (scores == null)
        {
            return new List<MatchRankingWidgetData>();
        }

        return scores.Players.Select(score =>
                new MatchRankingWidgetData { Position = score.Rank, Points = score.MatchPoints, Name = score.Name })
            .ToList();
    }

    public async Task HideManialink()
    {
        await _manialinkManager.HideManialinkAsync("MatchRankingModule.MatchRanking");
    }

    public Task Reset()
    {
        return _matchRankingStore.ResetScores();
    }
}
