using EvoSC.Common.Interfaces;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.LiveRankingModule.Models;
using EvoSC.Modules.Official.WorldRecordModule.Events;
using EvoSC.Modules.Official.WorldRecordModule.Interfaces;
using EvoSC.Modules.Official.WorldRecordModule.Models;
using Flurl;
using Flurl.Http;
using GbxRemoteNet.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.WorldRecordModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class WorldRecordService : IWorldRecordService
{
    private readonly ILogger<WorldRecordService> _logger;
    private readonly IEventManager _events;
    private readonly IServerClient _client;
    private WorldRecord? _currentWorldRecord;
    private readonly object _currentWorldRecordLock = new();

    public WorldRecordService(ILogger<WorldRecordService> logger, IEventManager events, IServerClient client)
    {
        _logger = logger;
        _events = events;
        _client = client;
    }

    public async Task FetchRecord(string mapUid)
    {
        TMioLeaderboardResponse? res = null;
        try
        {
            res = await "https://trackmania.io"
                .AppendPathSegments("api", "leaderboard", "map", mapUid)
                .WithHeaders(new
                {
                    User_Agent = "EvoSC# / World Record Grabber / Discord: chris92"
                })
                .GetJsonAsync<TMioLeaderboardResponse>();

        }
        catch (FlurlHttpException ex)
        {
            _logger.LogError(ex, "Invalid response from Openplanet. Maybe API issues?");
        }
        _logger.LogDebug("Loaded records for map.");


        if (res is {tops.Count: > 0})
        {
            var bestRecord = res.tops.First();
            var newWorldRecord = new WorldRecord
            {
                Name = bestRecord.player.name, Time = bestRecord.time, Source = "tm.io"
            };

            _logger.LogTrace("New world record loaded from tm.io: {name} -> {time}", newWorldRecord.Name,
                newWorldRecord.FormattedTime());
            await OverwriteRecord(newWorldRecord);
        }
        else
        {
            var mapInfo = await _client.Remote.GetCurrentMapInfoAsync();
            var author = mapInfo.AuthorNickname.Length > 0 ? mapInfo.AuthorNickname : mapInfo.Author;
            var newWorldRecord = new WorldRecord
            {
                Name = mapInfo.Author, Time = mapInfo.AuthorTime, Source = "AT"
            };
            
            _logger.LogDebug("Couldn't load World Record, using Author Time instead.");
            await OverwriteRecord(newWorldRecord);
        }
        
    }

    public async Task OverwriteRecord(WorldRecord newRecord)
    {
        lock (_currentWorldRecordLock)
        {
            _currentWorldRecord = newRecord;
        }

        await _events.RaiseAsync(WorldRecordEvents.NewRecord, new WorldRecordLoaded
        {
            Record = newRecord
        });
    }

    public Task ClearRecord()
    {
        lock (_currentWorldRecordLock)
        {
            _currentWorldRecord = null;
        }

        return Task.CompletedTask;
    }

    public Task<WorldRecord?> GetRecord()
    {
        return Task.FromResult(_currentWorldRecord);
    }

    public async Task DetectNewWorldRecordThroughScores(ScoresEventArgs scoresEventArgs)
    {
        if (_currentWorldRecord == null)
        {
            return;
        }

        foreach (var score in scoresEventArgs.Players)
        {
            if (score != null && score.BestRaceTime < _currentWorldRecord.Time)
            {
                await OverwriteRecord(new WorldRecord { Name = score.Name, Time = score.BestRaceTime, Source = "local" });
            }
        }
    }
}
