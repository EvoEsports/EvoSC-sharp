using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Callbacks;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Config;
using EvoSC.Modules.Official.LiveRankingModule.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Models;

namespace EvoSC.Modules.Official.LiveRankingModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class LiveRankingService(
    IManialinkManager manialinkManager,
    IServerClient server,
    ILiveRankingSettings settings,
    IPlayerManagerService playerManager,
    IMatchSettingsService matchSettingsService
) : ILiveRankingService
{
    private const string WidgetTemplate = "LiveRankingModule.LiveRanking";
    private bool _isPointsBased;

    public async Task DetectModeAndRequestScoreAsync()
    {
        _isPointsBased = await matchSettingsService.GetCurrentModeAsync() is not DefaultModeScriptName.TimeAttack;
        await RequestScoresAsync();
    }

    public Task RequestScoresAsync()
        => server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.GetScores");

    public async Task MapScoresAndSendWidgetAsync(ScoresEventArgs scores)
    {
        await manialinkManager.SendPersistentManialinkAsync(WidgetTemplate,
            new { settings, isPointsBased = _isPointsBased, scores = await MapScores(scores) });
    }

    public Task<IEnumerable<LiveRankingPosition>> MapScores(ScoresEventArgs scores)
    {
        return Task.FromResult(
            scores.Players.Take(settings.MaxWidgetRows)
                .Where(score => score != null)
                .OfType<PlayerScore>()
                .Where(score => ScoreShouldBeDisplayedAsync(score).Result)
                .Select(score => PlayerScoreToLiveRankingPositionAsync(score).Result)
        );
    }

    public Task HideWidgetAsync()
        => manialinkManager.HideManialinkAsync(WidgetTemplate);

    public Task<bool> ScoreShouldBeDisplayedAsync(PlayerScore score) =>
        Task.FromResult((_isPointsBased ? score.MatchPoints : score.BestRaceTime) > 0);

    public Task<bool> CurrentModeIsPointsBasedAsync()
        => Task.FromResult(_isPointsBased);

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
}
