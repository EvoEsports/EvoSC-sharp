using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Models;
using EvoSC.Modules.Official.LiveRankingModule.Utils;
using Flurl;
using Flurl.Http;
using GbxRemoteNet.Events;
using GbxRemoteNet.Structs;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.LiveRankingModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class LiveRankingService : ILiveRankingService
{
    private readonly ILogger<LiveRankingService> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IManialinkManager _manialinkManager;
    private readonly LiveRankingStore _liveRankingStore;
    private readonly IServerClient _client;
    private readonly IPlayerManagerService _playerManager;
    private bool isRoundsMode = false;

    public LiveRankingService(ILogger<LiveRankingService> logger, ILoggerFactory loggerFactory,
        IManialinkManager manialinkManager, IServerClient client, IPlayerManagerService playerManager)
    {
        _logger = logger;
        _loggerFactory = loggerFactory;
        _manialinkManager = manialinkManager;
        _client = client;
        _playerManager = playerManager;
        _liveRankingStore =
            new LiveRankingStore(_loggerFactory.CreateLogger<LiveRankingStore>(), _playerManager);
    }

    public async Task OnEnableAsync()
    {
        _logger.LogInformation("LiveRankingModule enabled");
        await CheckIsRoundsModeAsync();
        // if (isRoundsMode)
        // {
        //     var map = await _client.Remote.GetCurrentMapInfoAsync();
        //     await GetWorldRecordViaTMioAsync(map.UId, map.Name);
        //     await _liveRankingStore.ResetLiveRankingsAsync();
        //     _liveRankingStore.ResetRoundCounter();
        //     _liveRankingStore.IncreaseRoundCounter();
        //     _liveRankingStore.IncreaseTrackCounter();
        //     // var prevRanking = await _liveRankingStore.GetFullPreviousLiveRankingAsync();
        //     var curRanking = await _liveRankingStore.GetFullLiveRankingAsync();
        //
        //     _liveRankingStore.SortLiveRanking();
        //     
        //     //Map ranking entries for widget
        //     // var widgetPreviousRanking = GetLiveRankingForWidget(prevRanking);
        //     var widgetPreviousRanking = new List<LiveRankingWidgetPosition>();
        //     var widgetCurrentRanking = GetLiveRankingForWidget(curRanking);
        //     
        //     //Split current ranking into previously existing and new players
        //     var widgetExistingRanking = widgetCurrentRanking.Except(widgetPreviousRanking, new RankingComparer()).ToList();
        //     var widgetNewRanking = widgetCurrentRanking.Except(widgetExistingRanking, new RankingComparer()).ToList();
        //
        //     await _manialinkManager.SendPersistentManialinkAsync("LiveRankingModule.LiveRanking",
        //         new
        //         {
        //             previousRankings = widgetPreviousRanking,
        //             rankingsExisting = widgetExistingRanking,
        //             rankingsNew = widgetNewRanking
        //         });
        //     /*await _manialinkManager.SendPersistentManialinkAsync("LiveRankingModule.MatchInfo",
        //         new { data = _liveRankingStore.GetMatchInfo() });*/
        // }

        await Task.CompletedTask;
    }

    public async Task OnDisableAsync()
    {
        _logger.LogInformation("LiveRankingModule disabled");
        await _manialinkManager.HideManialinkAsync("LiveRankingModule.LiveRanking");
        // await _manialinkManager.HideManialinkAsync("LiveRankingModule.MatchInfo");
        await Task.CompletedTask;
    }

    public async Task OnPlayerWaypointAsync(WayPointEventArgs args)
    {
        await CheckIsRoundsModeAsync();
        if (isRoundsMode)
        {
            _logger.LogInformation("Player crossed a checkpoint: {ArgsAccountId} - RoundsMode: {IsRoundsMode}",
                args.AccountId, isRoundsMode);

            var previousRanking = (await _liveRankingStore.GetFullLiveRankingAsync()).ToList();
            _liveRankingStore.RegisterTime(args.AccountId, args.CheckpointInRace, args.RaceTime, args.IsEndRace);
            _liveRankingStore.SortLiveRanking();
            var currentRanking = await _liveRankingStore.GetFullLiveRankingAsync();

            await CalculateDiffs(currentRanking);

            //Map ranking entries for widget
            var widgetPreviousRanking = GetLiveRankingForWidget(previousRanking);
            var widgetCurrentRanking = GetLiveRankingForWidget(currentRanking);

            //Split current ranking into previously existing and new players
            var widgetExistingRanking = widgetCurrentRanking
                .Where(cr => widgetPreviousRanking.Contains(cr, new RankingComparer())).ToList();
            var widgetNewRanking = widgetCurrentRanking.Except(widgetExistingRanking).ToList();

            await _manialinkManager.SendManialinkAsync("LiveRankingModule.LiveRanking",
                new
                {
                    previousRankings = widgetPreviousRanking,
                    rankingsExisting = widgetExistingRanking,
                    rankingsNew = widgetNewRanking
                });
        }
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
        if (isRoundsMode)
        {
            _logger.LogInformation("Player gave up: {ArgsAccountId} - RoundsMode: {IsRoundsMode}", args.AccountId,
                isRoundsMode);
            _liveRankingStore.RegisterPlayerGiveUp(args.AccountId);
            var liveRanking = await _liveRankingStore.GetFullLiveRankingAsync();
            var widgetLiveRanking = GetLiveRankingForWidget(liveRanking);
            await _manialinkManager.SendPersistentManialinkAsync("LiveRankingModule.LiveRanking",
                new { liverankings = widgetLiveRanking });
        }
    }

    public async Task OnBeginMapAsync(MapEventArgs args)
    {
        _logger.LogInformation("Map starts: {MapName}", args.Map.Name);
        await CheckIsRoundsModeAsync();
        _logger.LogInformation("Is rounds mode on map start? {IsRoundsMode}", isRoundsMode);
        if (!isRoundsMode)
        {
            await Task.CompletedTask;
        }
        else
        {
            await GetWorldRecordViaTMioAsync(args.Map.Uid, args.Map.Name);
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
        _logger.LogInformation("Map ends: {MapName} - RoundsMode: {IsRoundsMode}", args.Map.Name, isRoundsMode);
        if (isRoundsMode)
        {
            await _liveRankingStore.ResetLiveRankingsAsync();
            await _manialinkManager.HideManialinkAsync("LiveRankingModule.LiveRanking");
            await _manialinkManager.HideManialinkAsync("LiveRankingModule.MatchInfo");
        }
    }

    public async Task OnStartRoundAsync(RoundEventArgs args)
    {
        await CheckIsRoundsModeAsync();
        _logger.LogInformation("Round {ArgsCount} starts - RoundsMode: {IsRoundsMode}", args.Count, isRoundsMode);
        await _liveRankingStore.ResetLiveRankingsAsync();
        var liveRanking = await _liveRankingStore.GetFullLiveRankingAsync();
        var widgetLiveRanking = GetLiveRankingForWidget(liveRanking);
        await _manialinkManager.SendPersistentManialinkAsync("LiveRankingModule.LiveRanking",
            new { liverankings = widgetLiveRanking });
        /*await _manialinkManager.SendPersistentManialinkAsync("LiveRankingModule.MatchInfo",
            new { data = _liveRankingStore.GetMatchInfo() });*/
    }

    public async Task OnEndRoundAsync(RoundEventArgs args)
    {
        _logger.LogInformation("Round {ArgsCount} ends - RoundsMode: {IsRoundsMode}", args.Count, isRoundsMode);
        await CheckIsRoundsModeAsync();
        var liveRanking = await _liveRankingStore.GetFullLiveRankingAsync();
        var nbFinished = liveRanking.FindAll(x => x.isFinish);
        if (nbFinished.Count > 0)
        {
            _logger.LogInformation("MatchInfo Rounds before: {ArgsCount}", _liveRankingStore.GetMatchInfo().NumRound);
            _liveRankingStore.IncreaseRoundCounter();
            _logger.LogInformation("MatchInfo Rounds after: {ArgsCount}", _liveRankingStore.GetMatchInfo().NumRound);
        }
    }

    public Task OnBeginMatchAsync()
    {
        throw new NotImplementedException();
    }

    public Task OnEndMatchAsync(EndMatchGbxEventArgs args)
    {
        throw new NotImplementedException();
    }

    public async Task OnPodiumStartAsync(PodiumEventArgs args)
    {
        await CheckIsRoundsModeAsync();
        await _manialinkManager.HideManialinkAsync("LiveRankingModule.LiveRanking");
        // await _manialinkManager.HideManialinkAsync("LiveRankingModule.MatchInfo");
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

        if (!ranking.isDNF)
        {
            var isDeltaTime = i > 0;
            var timeToFormat = isDeltaTime ? ranking.diffToFirst : ranking.cpTime;
            formattedTime = FormatTime(timeToFormat, isDeltaTime);
        }

        return new LiveRankingWidgetPosition(i + 1, ranking.player, formattedTime);
    }

    private async Task<bool> CheckIsRoundsModeAsync()
    {
        List<string> validModes = new List<string>
        {
            "Trackmania/TM_Rounds_Online.Script.txt", "Trackmania/TM_Cup_Online.Script.txt"
        };
        var modeScriptInfo = await _client.Remote.GetModeScriptInfoAsync();
        // isRoundsMode = validModes.Contains(modeScriptInfo.Name);
        isRoundsMode = true;
        return isRoundsMode;
    }

    private async Task GetWorldRecordViaTMioAsync(string mapUid, string mapName)
    {
        //TODO: outsource to own module so we can re-use it
        TMioLeaderboardResponse res = await "https://trackmania.io"
            .AppendPathSegments("api", "leaderboard", "map", mapUid)
            .WithHeaders(new { User_Agent = "EvoSC# / World Record Grabber / Discord: chris92" })
            .GetJsonAsync<TMioLeaderboardResponse>();
        _liveRankingStore.SetCurrentMap(mapName);
        _liveRankingStore.SetWorldRecord(res.tops[0].player.name, FormatTime(res.tops[0].time, false));
    }

    public MatchInfo GetMatchInfo()
    {
        return _liveRankingStore.GetMatchInfo();
    }
}
