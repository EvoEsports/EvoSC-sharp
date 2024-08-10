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
            new { settings, isPointsBased = _isPointsBased, scores = await MapScoresAsync(scores) });
    }

    public Task<IEnumerable<LiveRankingPosition>> MapScoresAsync(ScoresEventArgs scores)
    {
        return Task.FromResult(
            scores.Players.Take(settings.MaxWidgetRows)
                .Where(score => score != null)
                .OfType<PlayerScore>()
                .Where(ScoreShouldBeDisplayed)
                .Select(PlayerScoreToLiveRankingPosition)
        );
    }

    public Task HideWidgetAsync()
        => manialinkManager.HideManialinkAsync(WidgetTemplate);

    public Task<bool> CurrentModeIsPointsBasedAsync()
        => Task.FromResult(_isPointsBased);

    public bool ScoreShouldBeDisplayed(PlayerScore score)
    {
        if (_isPointsBased)
        {
            return score.MatchPoints > 0;
        }

        return score.BestRaceTime > 0;
    }

    public LiveRankingPosition PlayerScoreToLiveRankingPosition(PlayerScore score)
    {
        var player = playerManager.GetPlayerAsync(score.AccountId).Result;
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
