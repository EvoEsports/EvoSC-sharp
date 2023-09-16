using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Models;
using EvoSC.Modules.Official.LiveRankingModule.Utils;
using GbxRemoteNet.Events;
using LinqToDB.Common;
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
    private bool _isRoundsMode;

    public LiveRankingService(ILogger<LiveRankingService> logger, ILoggerFactory loggerFactory,
        IManialinkManager manialinkManager, IServerClient client, IPlayerManagerService playerManager,
        IEvoScBaseConfig config)
    {
        _logger = logger;
        _manialinkManager = manialinkManager;
        _client = client;
        _config = config;
        _liveRankingStore =
            new LiveRankingStore(loggerFactory.CreateLogger<LiveRankingStore>(), playerManager);
    }

    public async Task OnEnableAsync()
    {
        _logger.LogInformation("LiveRankingModule enabled");
        await CheckIsRoundsModeAsync();
        if (_isRoundsMode)
        {
            await _liveRankingStore.ResetLiveRankingsAsync();
            _liveRankingStore.ResetRoundCounter();
            _liveRankingStore.IncreaseRoundCounter();
            _liveRankingStore.IncreaseTrackCounter();

            await _manialinkManager.SendPersistentManialinkAsync("LiveRankingModule.LiveRanking", await GetWidgetData());
        }

        await Task.CompletedTask;
    }

    public async Task OnDisableAsync()
    {
        _logger.LogInformation("LiveRankingModule disabled");
        await _manialinkManager.HideManialinkAsync("LiveRankingModule.LiveRanking");
        await Task.CompletedTask;
    }

    public async Task OnPlayerWaypointAsync(WayPointEventArgs args)
    {
        await CheckIsRoundsModeAsync();
        if (_isRoundsMode)
        {
            _logger.LogInformation("Player crossed a checkpoint: {ArgsAccountId} - RoundsMode: {IsRoundsMode}",
                args.AccountId, _isRoundsMode);

            var previousRanking = (await _liveRankingStore.GetFullLiveRankingAsync()).ToList();
            _liveRankingStore.RegisterTime(args.AccountId, args.CheckpointInRace, args.RaceTime, args.IsEndRace);

            await _manialinkManager.SendPersistentManialinkAsync("LiveRankingModule.LiveRanking",
                await GetWidgetData(previousRanking));
        }
    }

    private async Task<dynamic> GetWidgetData(List<ExpandedLiveRankingPosition>? previousRanking = null)
    {
        var currentRanking = (await _liveRankingStore.GetFullLiveRankingAsync()).Take(ShowRows).ToList();
        await CalculateDiffs(currentRanking);
        var widgetCurrentRanking = GetLiveRankingForWidget(currentRanking);

        if (previousRanking.IsNullOrEmpty())
        {
            return new
            {
                previousRankings = widgetCurrentRanking,
                rankingsExisting = new List<LiveRankingWidgetPosition>(),
                rankingsNew = new List<LiveRankingWidgetPosition>(),
                headerColor = _config.Theme.UI.HeaderBackgroundColor,
                primaryColor = _config.Theme.UI.PrimaryColor,
                logoUrl = _config.Theme.UI.LogoWhiteUrl,
                playerRowBackgroundColor = _config.Theme.UI.PlayerRowBackgroundColor
            };
        }

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
    }

    public Task CalculateDiffs(List<ExpandedLiveRankingPosition> rankings)
    {
        //TODO: calculate diffs correctly over different cp indexes

        if (rankings.Count > 0)
        {
            var firstRanking = rankings.First();
            foreach (var ranking in rankings)
            {
                ranking.diffToFirst = firstRanking.cpTime - ranking.cpTime;
            }
        }

        return Task.CompletedTask;
    }

    public async Task OnPlayerGiveupAsync(PlayerUpdateEventArgs args)
    {
        await CheckIsRoundsModeAsync();
        if (_isRoundsMode)
        {
            _logger.LogInformation("Player gave up: {ArgsAccountId} - RoundsMode: {IsRoundsMode}", args.AccountId,
                _isRoundsMode);

            var previousRanking = (await _liveRankingStore.GetFullLiveRankingAsync()).ToList();
            _liveRankingStore.RegisterPlayerGiveUp(args.AccountId);

            await _manialinkManager.SendPersistentManialinkAsync("LiveRankingModule.LiveRanking",
                await GetWidgetData(previousRanking));
        }
    }

    public async Task OnBeginMapAsync(MapEventArgs args)
    {
        _logger.LogInformation("Map starts: {MapName}", args.Map.Name);
        await CheckIsRoundsModeAsync();
        _logger.LogInformation("Is rounds mode on map start? {IsRoundsMode}", _isRoundsMode);
        if (!_isRoundsMode)
        {
            await Task.CompletedTask;
        }
        else
        {
            _liveRankingStore.ResetRoundCounter();
            _liveRankingStore.IncreaseRoundCounter();
            _liveRankingStore.IncreaseTrackCounter();
            /*await _manialinkManager.SendPersistentManialinkAsync("LiveRankingModule.MatchInfo",
                new { data = _liveRankingStore.GetMatchInfo() });*/
            await _liveRankingStore.ResetLiveRankingsAsync();
        }
    }

    public async Task OnEndMapAsync(MapEventArgs args)
    {
        await CheckIsRoundsModeAsync();
        _logger.LogInformation("Map ends: {MapName} - RoundsMode: {IsRoundsMode}", args.Map.Name, _isRoundsMode);
        if (_isRoundsMode)
        {
            await _liveRankingStore.ResetLiveRankingsAsync();
            await _manialinkManager.HideManialinkAsync("LiveRankingModule.LiveRanking");
            await _manialinkManager.HideManialinkAsync("LiveRankingModule.MatchInfo");
        }
    }

    public async Task ResetLiveRanking()
    {
        await _liveRankingStore.ResetLiveRankingsAsync();
    }

    public async Task OnStartRoundAsync(RoundEventArgs args)
    {
        _logger.LogInformation("Round {ArgsCount} starts - RoundsMode: {IsRoundsMode}", args.Count, _isRoundsMode);
        await _liveRankingStore.ResetLiveRankingsAsync();
        await _manialinkManager.SendPersistentManialinkAsync("LiveRankingModule.LiveRanking", await GetWidgetData());
    }

    public async Task SendManialink()
    {
        await _manialinkManager.SendPersistentManialinkAsync("LiveRankingModule.LiveRanking", await GetWidgetData());
    }

    public async Task OnEndRoundAsync(RoundEventArgs args)
    {
        _logger.LogInformation("Round {ArgsCount} ends - RoundsMode: {IsRoundsMode}", args.Count, _isRoundsMode);
        var liveRanking = await _liveRankingStore.GetFullLiveRankingAsync();
        var nbFinished = liveRanking.FindAll(x => x.isFinish);
        if (nbFinished.Count > 0)
        {
            _logger.LogInformation("MatchInfo Rounds before: {ArgsCount}", _liveRankingStore.GetMatchInfo().NumRound);
            _liveRankingStore.IncreaseRoundCounter();
            _logger.LogInformation("MatchInfo Rounds after: {ArgsCount}", _liveRankingStore.GetMatchInfo().NumRound);
        }
    }

    public async Task OnBeginMatchAsync()
    {
        await SendManialink();
    }

    public async Task OnEndMatchAsync(EndMatchGbxEventArgs args)
    {
        await HideManialink();
    }

    public async Task OnPodiumStartAsync(PodiumEventArgs args)
    {
        await CheckIsRoundsModeAsync();
        await HideManialink();
        // await _manialinkManager.HideManialinkAsync("LiveRankingModule.MatchInfo");
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

        if (ranking.isDNF)
        {
            return new LiveRankingWidgetPosition(i + 1, ranking.player, formattedTime, ranking.cpIndex + 1, ranking.isFinish);
        }

        var isDeltaTime = i > 0;
        var timeToFormat = isDeltaTime ? ranking.diffToFirst : ranking.cpTime;
        formattedTime = FormatTime(timeToFormat, isDeltaTime);

        return new LiveRankingWidgetPosition(i + 1, ranking.player, formattedTime, ranking.cpIndex + 1, ranking.isFinish);
    }

    private async Task<bool> CheckIsRoundsModeAsync()
    {
        List<string> validModes = new List<string>
        {
            "Trackmania/TM_Rounds_Online.Script.txt", "Trackmania/TM_Cup_Online.Script.txt"
        };
        var modeScriptInfo = await _client.Remote.GetModeScriptInfoAsync();
        // isRoundsMode = validModes.Contains(modeScriptInfo.Name);
        _isRoundsMode = true;
        return _isRoundsMode;
    }

    public MatchInfo GetMatchInfo()
    {
        return _liveRankingStore.GetMatchInfo();
    }
}
