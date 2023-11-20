using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Models;
using EvoSC.Modules.Official.LiveRankingModule.Utils;
using EvoSC.Modules.Official.MatchRankingModule.Interfaces;
using EvoSC.Modules.Official.MatchRankingModule.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.MatchRankingModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class MatchRankingService : IMatchRankingService
{
    private const int ShowRows = 4;
    private const string Template = "MatchRankingModule.MatchRanking";

    private readonly IManialinkManager _manialinkManager;
    private readonly IPlayerManagerService _playerManager;
    private readonly ILogger<MatchRankingService> _logger;
    private readonly IEvoScBaseConfig _config;
    private readonly IThemeManager _themes;
    private MatchRankingStore _matchRankingStore;

    public MatchRankingService(IManialinkManager manialinkManager, IPlayerManagerService playerManager,
        ILogger<MatchRankingService> logger, IEvoScBaseConfig config, IThemeManager themes)
    {
        _manialinkManager = manialinkManager;
        _playerManager = playerManager;
        _logger = logger;
        _config = config;
        _themes = themes;
        _matchRankingStore = new MatchRankingStore();
    }

    public Task UpdateAndShowScores(ScoresEventArgs scores)
    {
        _matchRankingStore.ConsumeScores(scores);
        return SendManialinkToPlayers();
    }

    public async Task SendManialinkToPlayers()
    {
        var players = await _playerManager.GetOnlinePlayersAsync();

        foreach (var player in players)
        {
            if (player.State == PlayerState.Spectating)
            {
                await _manialinkManager.SendManialinkAsync(player, Template, await GetWidgetData());
            }
        }
    }

    public async Task SendManialinkToPlayer(string accountId)
    {
        var player = await _playerManager.GetOnlinePlayerAsync(accountId);

        if (player.State == PlayerState.Spectating)
        {
            await _manialinkManager.SendManialinkAsync(player, Template, await GetWidgetData());
        }
    }

    private async Task<dynamic> GetWidgetData()
    {
        //var mappedScoresPrevious = (await MapScoresForWidget(_matchRankingStore.GetPreviousMatchScores())).ToList()
        //    .Take(ShowRows).ToList();
        var mappedScoresLatest = (await MapScoresForWidget(_matchRankingStore.GetLatestMatchScores())).ToList()
            .Take(ShowRows).ToList();

        //var mappedScoresExisting = mappedScoresLatest
        //    .Where(ranking => mappedScoresPrevious.Contains(ranking, new RankingComparer())).ToList();
        //var mappedScoresNew = mappedScoresLatest.Except(mappedScoresExisting).ToList();

        return new
        {
            Scores = mappedScoresLatest,
            headerColor = _themes.Theme.UI_HeaderBackgroundColor,
            primaryColor = _themes.Theme.UI_PrimaryColor,
            logoUrl = _themes.Theme.UI_LogoWhiteUrl,
            playerRowBackgroundColor = _themes.Theme.UI_PlayerRowBackgroundColor
        };
    }

    public async Task HideManialink()
    {
        await _manialinkManager.HideManialinkAsync(Template);
    }

    public Task ResetMatchData()
    {
        _logger.LogTrace("Clearing match ranking data.");
        _matchRankingStore = new MatchRankingStore();

        return Task.CompletedTask;
    }

    public async Task HandlePlayerStateChange(string accountId)
    {
        var player = await _playerManager.GetOnlinePlayerAsync(accountId);
        if (player.State == PlayerState.Playing)
        {
            await _manialinkManager.HideManialinkAsync(Template);
            return;
        }

        await _manialinkManager.SendManialinkAsync(player, Template, await GetWidgetData());
    }

    private async Task<IEnumerable<LiveRankingWidgetPosition>> MapScoresForWidget(ScoresEventArgs? scores)
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

            var player = await _playerManager.GetPlayerAsync(score.AccountId);

            if (player == null)
            {
                continue;
            }

            playerScores.Add(new LiveRankingWidgetPosition(
                score.Rank,
                player,
                player.GetLogin(),
                (score.MatchPoints + score.RoundPoints).ToString(),
                -1,
                false
            ));
        }

        return playerScores;
    }
}
