using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Models;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.LiveRankingModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class LiveRankingService : ILiveRankingService
{
    private const int ShowRows = 4;

    private readonly ILogger<LiveRankingService> _logger;
    private readonly IManialinkManager _manialinkManager;
    private readonly LiveRankingStore _liveRankingStore;
    private readonly IServerClient _client;
    private readonly IEvoScBaseConfig _config;
    private readonly IThemeManager _themes;
    private bool _isRoundsMode;

    public LiveRankingService(ILogger<LiveRankingService> logger, ILoggerFactory loggerFactory,
        IManialinkManager manialinkManager, IServerClient client, IPlayerManagerService playerManager,
        IEvoScBaseConfig config, IThemeManager themes)
    {
        _logger = logger;
        _manialinkManager = manialinkManager;
        _client = client;
        _config = config;
        _themes = themes;
        _liveRankingStore =
            new LiveRankingStore(loggerFactory.CreateLogger<LiveRankingStore>(), playerManager);
    }

    public async Task OnEnableAsync()
    {
        _logger.LogTrace("LiveRankingModule enabled");
        await CheckIsRoundsModeAsync();
        await HideNadeoScoreboard();
        if (_isRoundsMode)
        {
            await _liveRankingStore.ResetLiveRankingsAsync();
            _liveRankingStore.ResetRoundCounter();
            _liveRankingStore.IncreaseRoundCounter();
            _liveRankingStore.IncreaseTrackCounter();

            await _manialinkManager.SendPersistentManialinkAsync("LiveRankingModule.LiveRanking",
                await GetWidgetData());
        }

        await Task.CompletedTask;
    }

    public async Task OnDisableAsync()
    {
        _logger.LogTrace("LiveRankingModule disabled");
        await _manialinkManager.HideManialinkAsync("LiveRankingModule.LiveRanking");
        await Task.CompletedTask;
    }

    public async Task OnPlayerWaypointAsync(WayPointEventArgs args)
    {
        await CheckIsRoundsModeAsync();
        if (_isRoundsMode)
        {
            _logger.LogTrace("Player crossed a checkpoint: {ArgsAccountId} - RoundsMode: {IsRoundsMode}",
                args.AccountId, _isRoundsMode);

            //var previousRanking = (await _liveRankingStore.GetFullLiveRankingAsync()).ToList();
            _liveRankingStore.RegisterTime(args.AccountId, args.CheckpointInRace, args.RaceTime, args.IsEndRace);

            await _manialinkManager.SendPersistentManialinkAsync("LiveRankingModule.LiveRanking",
                await GetWidgetData());
        }
    }

    private async Task<dynamic> GetWidgetData()
    {
        var currentRanking = (await _liveRankingStore.GetFullLiveRankingAsync()).Take(ShowRows).ToList();
        await CalculateDiffsAsync(currentRanking);
        var widgetCurrentRanking = GetLiveRankingForWidget(currentRanking);

        return new
        {
            rankings = widgetCurrentRanking,
            headerColor = _themes.Theme.UI_HeaderBackgroundColor,
            primaryColor = _themes.Theme.UI_PrimaryColor,
            logoUrl = _themes.Theme.UI_LogoWhiteUrl,
            playerRowBackgroundColor = _themes.Theme.UI_PlayerRowBackgroundColor
        };

/*
        //Map ranking entries for widget
        var widgetPreviousRanking = GetLiveRankingForWidget(previousRanking.Take(ShowRows).ToList());

        //Split current ranking into previously existing and new players
        var widgetExistingRanking = widgetCurrentRanking
            .Where(cr => widgetPreviousRanking.Contains(cr, new RankingComparer())).ToList();
        var widgetNewRanking = widgetCurrentRanking.Except(widgetExistingRanking).ToList();

        return new
        {
            previousRankings = widgetPreviousRanking,
            rankingsExisting = widgetExistingRanking,
            rankingsNew = widgetNewRanking,
            headerColor = _config.Theme.UI.HeaderBackgroundColor,
            primaryColor = _config.Theme.UI.PrimaryColor,
            logoUrl = _config.Theme.UI.LogoWhiteUrl,
            playerRowBackgroundColor = _config.Theme.UI.PlayerRowBackgroundColor
        };
        */
    }

    public Task CalculateDiffsAsync(List<ExpandedLiveRankingPosition> rankings)
    {
        if (rankings.Count > 0)
        {
            var firstRanking = rankings.First();
            foreach (var ranking in rankings)
            {
                ranking.DiffToFirstPosition = firstRanking.CheckpointTime - ranking.CheckpointTime;
            }
        }

        return Task.CompletedTask;
    }

    public async Task OnPlayerGiveupAsync(PlayerUpdateEventArgs args)
    {
        await CheckIsRoundsModeAsync();
        if (_isRoundsMode)
        {
            _logger.LogTrace("Player gave up: {ArgsAccountId} - RoundsMode: {IsRoundsMode}", args.AccountId,
                _isRoundsMode);

            //var previousRanking = (await _liveRankingStore.GetFullLiveRankingAsync()).ToList();
            _liveRankingStore.RegisterPlayerGiveUp(args.AccountId);

            await _manialinkManager.SendPersistentManialinkAsync("LiveRankingModule.LiveRanking",
                await GetWidgetData());
        }
    }

    public async Task OnBeginMapAsync(MapEventArgs args)
    {
        _logger.LogTrace("Map starts: {MapName}, IsRounds: {IsRoundsMode}", args.Map.Name, _isRoundsMode);
        await CheckIsRoundsModeAsync();
        if (!_isRoundsMode)
        {
            await Task.CompletedTask;
        }
        else
        {
            _liveRankingStore.ResetRoundCounter();
            _liveRankingStore.IncreaseRoundCounter();
            _liveRankingStore.IncreaseTrackCounter();
            await _liveRankingStore.ResetLiveRankingsAsync();
        }
    }

    public async Task OnEndMapAsync(MapEventArgs args)
    {
        await CheckIsRoundsModeAsync();
        _logger.LogTrace("Map ends: {MapName} - RoundsMode: {IsRoundsMode}", args.Map.Name, _isRoundsMode);
        if (_isRoundsMode)
        {
            await _liveRankingStore.ResetLiveRankingsAsync();
            await _manialinkManager.HideManialinkAsync("LiveRankingModule.LiveRanking");
            await _manialinkManager.HideManialinkAsync("LiveRankingModule.MatchInfo");
        }
    }

    public async Task ResetLiveRankingAsync()
    {
        await _liveRankingStore.ResetLiveRankingsAsync();
    }

    public async Task OnStartRoundAsync(RoundEventArgs args)
    {
        _logger.LogTrace("Round {ArgsCount} starts - RoundsMode: {IsRoundsMode}", args.Count, _isRoundsMode);
        await _liveRankingStore.ResetLiveRankingsAsync();
        await _manialinkManager.SendPersistentManialinkAsync("LiveRankingModule.LiveRanking", await GetWidgetData());
    }

    public async Task SendManialinkAsync()
    {
        await _manialinkManager.SendPersistentManialinkAsync("LiveRankingModule.LiveRanking", await GetWidgetData());
    }

    public async Task OnEndRoundAsync(RoundEventArgs args)
    {
        _logger.LogTrace("Round {ArgsCount} ends - RoundsMode: {IsRoundsMode}", args.Count, _isRoundsMode);
        var liveRanking = await _liveRankingStore.GetFullLiveRankingAsync();
        var nbFinished = liveRanking.FindAll(x => x.IsFinish);
        if (nbFinished.Count > 0)
        {
            _logger.LogTrace("MatchInfo Rounds before: {ArgsCount}", _liveRankingStore.GetMatchInfo().NumRound);
            _liveRankingStore.IncreaseRoundCounter();
            _logger.LogTrace("MatchInfo Rounds after: {ArgsCount}", _liveRankingStore.GetMatchInfo().NumRound);
        }
    }

    public async Task OnBeginMatchAsync()
    {
        await SendManialinkAsync();
    }

    public async Task OnEndMatchAsync(EndMatchGbxEventArgs args)
    {
        await HideManialink();
    }

    public async Task OnPodiumStartAsync(PodiumEventArgs args)
    {
        await CheckIsRoundsModeAsync();
        await HideManialink();
    }

    public async Task HideNadeoScoreboard()
    {
        var hudSettings = new List<string>()
        {
            @"{
    ""uimodules"": [
        {
            ""id"": ""Rounds_SmallScoresTable"",
            ""visible"": false,
            ""visible_update"": true
        }
    ]
}"
        };

        await _client.Remote.TriggerModeScriptEventArrayAsync("Common.UIModules.SetProperties", hudSettings.ToArray());
    }

    private async Task HideManialink()
    {
        await _manialinkManager.HideManialinkAsync("LiveRankingModule.LiveRanking");
    }

    private string FormatTime(int time, bool isDelta)
    {
        TimeSpan ts = TimeSpan.FromMilliseconds(time);

        if (isDelta)
        {
            return $"+ {Math.Abs(ts.Seconds)}.{ts.ToString("fff")}";
        }

        if (time > 60_000)
        {
            return $"{ts.ToString(@"mm\:ss\.fff")}";
        }

        return $"{ts.ToString(@"ss\.fff")}";
    }

    private List<LiveRankingWidgetPosition> GetLiveRankingForWidget(List<ExpandedLiveRankingPosition> liveRanking)
    {
        return liveRanking.Select(RankingToTime).ToList();
    }

    private LiveRankingWidgetPosition RankingToTime(ExpandedLiveRankingPosition ranking, int i)
    {
        var formattedTime = "DNF";

        if (ranking.IsDnf)
        {
            return new LiveRankingWidgetPosition(i + 1, ranking.Player, ranking.Player.GetLogin(), formattedTime,
                ranking.CheckpointIndex + 1, ranking.IsFinish);
        }

        var isDeltaTime = i > 0;
        var timeToFormat = isDeltaTime ? ranking.DiffToFirstPosition : ranking.CheckpointTime;
        formattedTime = FormatTime(timeToFormat, isDeltaTime);

        return new LiveRankingWidgetPosition(i + 1, ranking.Player, ranking.Player.GetLogin(), formattedTime,
            ranking.CheckpointIndex + 1, ranking.IsFinish);
    }

    private Task<bool> CheckIsRoundsModeAsync()
    {
        List<string> validModes = new List<string>
        {
            "Trackmania/TM_Rounds_Online.Script.txt", "Trackmania/TM_Cup_Online.Script.txt"
        };
        //TODO: implement
        _isRoundsMode = true;
        return Task.FromResult(_isRoundsMode);
    }

    public MatchInfo GetMatchInfo()
    {
        return _liveRankingStore.GetMatchInfo();
    }
}
