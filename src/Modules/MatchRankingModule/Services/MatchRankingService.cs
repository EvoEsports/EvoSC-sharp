using System.Globalization;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MatchRankingModule.Interfaces;
using EvoSC.Modules.Official.MatchRankingModule.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.MatchRankingModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class MatchRankingService(IManialinkManager manialinkManager, IPlayerManagerService playerManager,
        ILogger<MatchRankingService> logger)
    : IMatchRankingService
{
    private const int ShowRows = 4;
    private const string Template = "MatchRankingModule.MatchRanking";

    private MatchRankingStore _matchRankingStore = new();

    public Task UpdateAndShowScores(ScoresEventArgs scores)
    {
        _matchRankingStore.ConsumeScores(scores);
        return SendManialinkToPlayers();
    }

    public async Task SendManialinkToPlayers()
    {
        var players = await playerManager.GetOnlinePlayersAsync();

        foreach (var player in players)
        {
            if (player.State == PlayerState.Spectating)
            {
                await manialinkManager.SendManialinkAsync(player, Template, await GetWidgetDataAsync());
            }
        }
    }

    public async Task SendManialinkToPlayer(string accountId)
    {
        var player = await playerManager.GetOnlinePlayerAsync(accountId);

        if (player.State == PlayerState.Spectating)
        {
            await manialinkManager.SendManialinkAsync(player, Template, await GetWidgetDataAsync());
        }
    }

    private async Task<dynamic> GetWidgetDataAsync()
    {
        var mappedScoresLatest = (await MapScoresForWidgetAsync(_matchRankingStore.GetLatestMatchScores()))
            .Take(ShowRows)
            .ToList();

        return new
        {
            Scores = mappedScoresLatest
        };
    }

    public async Task HideManialink()
    {
        await manialinkManager.HideManialinkAsync(Template);
    }

    public Task ResetMatchData()
    {
        logger.LogTrace("Clearing match ranking data.");
        _matchRankingStore = new MatchRankingStore();

        return Task.CompletedTask;
    }

    public async Task HandlePlayerStateChange(string accountId)
    {
        var player = await playerManager.GetOnlinePlayerAsync(accountId);
        if (player.State == PlayerState.Playing)
        {
            await manialinkManager.HideManialinkAsync(Template);
            return;
        }

        await manialinkManager.SendManialinkAsync(player, Template, await GetWidgetDataAsync());
    }

    private async Task<IEnumerable<LiveRankingWidgetPosition>> MapScoresForWidgetAsync(ScoresEventArgs? scores)
    {
        if (scores == null)
        {
            return new List<LiveRankingWidgetPosition>();
        }

        var playerScores = new List<LiveRankingWidgetPosition>();
        foreach (var score in scores.Players)
        {
            if (score == null)
            {
                continue;
            }

            var player = await playerManager.GetPlayerAsync(score.AccountId);

            if (player == null)
            {
                continue;
            }

            playerScores.Add(new LiveRankingWidgetPosition(
                score.Rank,
                player,
                player.GetLogin(),
                (score.MatchPoints + score.RoundPoints).ToString(CultureInfo.InvariantCulture),
                -1,
                false
            ));
        }

        return playerScores;
    }
}
