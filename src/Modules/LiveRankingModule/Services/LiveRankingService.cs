using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Callbacks;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Config;
using EvoSC.Modules.Official.LiveRankingModule.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.LiveRankingModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class LiveRankingService(
    IManialinkManager manialinkManager,
    IServerClient server,
    ILiveRankingSettings settings,
    IPlayerManagerService playerManager,
    ILogger<LiveRankingService> logger
) : ILiveRankingService
{
    private const string WidgetTemplate = "LiveRankingModule.LiveRanking";

    public async Task InitializeAsync()
    {
        await server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.GetScores");
    }

    public async Task HandleScoresAsync(ScoresEventArgs scores)
    {
        var isTeamsMode = scores.UseTeams;
        var isPointsBased = isTeamsMode || scores.UseTeams; //TODO: detect rounds properly

        var liveRankingPositions = scores.Players.Take(settings.MaxWidgetRows)
            .Where(score => score != null)
            .OfType<PlayerScore>()
            .Where(score => ScoreShouldBeDisplayedAsync(score, isPointsBased).Result)
            .Select(score => PlayerScoreToLiveRankingPositionAsync(score).Result);

        await manialinkManager.SendPersistentManialinkAsync(WidgetTemplate,
            new { settings, isPointsBased, isTeamsMode, scores = liveRankingPositions });
    }

    public Task<bool> ScoreShouldBeDisplayedAsync(PlayerScore score, bool isPointsBased)
    {
        return Task.FromResult((isPointsBased ? score.MatchPoints : score.BestRaceTime) > 0);
    }

    public async Task<LiveRankingPosition> PlayerScoreToLiveRankingPositionAsync(PlayerScore score)
    {
        var player = await playerManager.GetPlayerAsync(score.AccountId);
        var nickname = score.Name;

        if (player != null)
        {
            nickname = player.NickName;
        }

        return new LiveRankingPosition(
            score.AccountId,
            nickname,
            score.Rank,
            score.BestRaceTime,
            score.MatchPoints
        );
    }

    public Task HideWidgetAsync()
        => manialinkManager.HideManialinkAsync(WidgetTemplate);
}
