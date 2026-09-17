using EvoSC.Common.Events;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Callbacks;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Manialinks.Vdom.Views;
using EvoSC.Modules.Official.LiveRankingModule.Config;
using EvoSC.Modules.Official.LiveRankingModule.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Models;
using EvoSC.Modules.Official.LiveRankingModule.Vdom;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.LiveRankingModule.Services;

/// <summary>
/// M1's vertical slice: this is the one service ported to the reactive engine
/// (<see cref="IManialinkViewManager"/>) instead of the string-render-and-resend
/// <c>IManialinkManager</c> path every other module still uses -- see the plan. The old
/// <c>LiveRankingModule.LiveRanking</c> .mt templates are left in place, unreferenced, as a
/// reference for later M3/M4 work rather than deleted.
/// </summary>
[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class LiveRankingService : ILiveRankingService
{
    private readonly IManialinkViewManager _viewManager;
    private readonly IServerClient _server;
    private readonly ILiveRankingSettings _settings;
    private readonly IPlayerManagerService _playerManager;
    private readonly IMatchSettingsService _matchSettingsService;

    private bool _isPointsBased;
    private IManialinkView? _view;

    public LiveRankingService(
        IManialinkViewManager viewManager,
        IServerClient server,
        ILiveRankingSettings settings,
        IPlayerManagerService playerManager,
        IMatchSettingsService matchSettingsService,
        IEventManager events)
    {
        _viewManager = viewManager;
        _server = server;
        _settings = settings;
        _playerManager = playerManager;
        _matchSettingsService = matchSettingsService;

        // The old IManialinkManager path gets this for free via SendPersistentManialinkAsync +
        // ManialinkManager's own PlayerConnect handler (ManialinkManager.cs:50). Bypassing that
        // manager means bypassing that behaviour too, so it's replicated here -- otherwise a
        // player joining mid-race would never see the widget at all, a real regression versus
        // today.
        events.Subscribe(s => s
            .WithEvent(GbxRemoteEvent.PlayerConnect)
            .WithInstance(this)
            .WithInstanceClass<LiveRankingService>()
            .WithHandlerMethod<PlayerConnectGbxEventArgs>(HandlePlayerConnectAsync)
            .AsAsync()
        );
    }

    public async Task DetectModeAndRequestScoreAsync()
    {
        _isPointsBased = await _matchSettingsService.GetCurrentModeAsync() is not DefaultModeScriptName.TimeAttack;
        await RequestScoresAsync();
    }

    public Task RequestScoresAsync()
        => _server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.GetScores");

    public async Task MapScoresAndSendWidgetAsync(ScoresEventArgs scores)
    {
        var props = new LiveRankingViewProps(_settings, _isPointsBased, (await MapScoresAsync(scores)).ToList());

        if (_view == null)
        {
            var view = new LiveRankingView(_settings.MaxWidgetRows);
            var players = await _playerManager.GetOnlinePlayersAsync();
            _view = await _viewManager.MountAsync(players, view, props);
        }
        else
        {
            await _view.UpdateAsync(props);
        }
    }

    public Task<IEnumerable<LiveRankingPosition>> MapScoresAsync(ScoresEventArgs scores)
    {
        return Task.FromResult(
            scores.Players.Take(_settings.MaxWidgetRows)
                .Where(score => score != null)
                .OfType<PlayerScore>()
                .Where(ScoreShouldBeDisplayed)
                .Select(PlayerScoreToLiveRankingPosition)
        );
    }

    public async Task HideWidgetAsync()
    {
        if (_view == null)
        {
            return;
        }

        await _view.UnmountAsync();
        _view = null;
    }

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
        var player = _playerManager.GetPlayerAsync(score.AccountId).Result;
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

    private async Task HandlePlayerConnectAsync(object sender, PlayerConnectGbxEventArgs e)
    {
        if (_view == null)
        {
            return;
        }

        var player = await _playerManager.GetOnlinePlayerAsync(PlayerUtils.ConvertLoginToAccountId(e.Login));
        await _view.AddPlayersAsync([player]);
    }
}
