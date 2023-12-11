using EvoSC.Common.Interfaces;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.LiveRankingModule.Models;
using EvoSC.Modules.Official.WorldRecordModule.Events;
using EvoSC.Modules.Official.WorldRecordModule.Interfaces;
using EvoSC.Modules.Official.WorldRecordModule.Models;
using Flurl;
using Flurl.Http;
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

    public async Task FetchRecordAsync(string mapUid)
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
            throw;
        }
        
        _logger.LogDebug("Loaded records for map.");


        if (res is {tops.Count: > 0})
        {
            var bestRecord = res.tops.First();
            var newWorldRecord = new WorldRecord
            {
                PlayerName = bestRecord.player.name,
                Time = RaceTime.FromMilliseconds(bestRecord.time),
                Source = WorldRecordSource.TrackmaniaIo
            };

            _logger.LogTrace("New world record loaded from tm.io: {name} -> {time}",
                newWorldRecord.PlayerName,
                newWorldRecord.Time.ToString()
            );
            
            await OverwriteRecordAsync(newWorldRecord);
        }
        else
        {
            var mapInfo = await _client.Remote.GetCurrentMapInfoAsync();
            var author = mapInfo.AuthorNickname.Length > 0 ? mapInfo.AuthorNickname : mapInfo.Author;
            var newWorldRecord = new WorldRecord
            {
                PlayerName = author,
                Time = RaceTime.FromMilliseconds(mapInfo.AuthorTime),
                Source = WorldRecordSource.AuthorTime
            };
            
            _logger.LogDebug("Couldn't load World Record, using Author Time instead.");
            
            await OverwriteRecordAsync(newWorldRecord);
        }
        
    }

    public Task ClearRecordAsync()
    {
        lock (_currentWorldRecordLock)
        {
            _currentWorldRecord = null;
        }

        return Task.CompletedTask;
    }

    public Task<WorldRecord?> GetRecordAsync()
    {
        return Task.FromResult(_currentWorldRecord);
    }

    public async Task DetectNewWorldRecordThroughScoresAsync(ScoresEventArgs scoresEventArgs)
    {
        if (_currentWorldRecord == null)
        {
            return;
        }

        foreach (var score in scoresEventArgs.Players)
        {
            if (score is { BestRaceTime: < 0 } && score.BestRaceTime < _currentWorldRecord.Time.TotalMilliseconds)
            {
                await OverwriteRecordAsync(new WorldRecord
                {
                    PlayerName = score.Name, 
                    Time = RaceTime.FromMilliseconds(score.BestRaceTime), 
                    Source = WorldRecordSource.Local
                });
            }
        }
    }
    
    private async Task OverwriteRecordAsync(WorldRecord newRecord)
    {
        lock (_currentWorldRecordLock)
        {
            _currentWorldRecord = newRecord;
        }

        await _events.RaiseAsync(WorldRecordEvents.NewRecord, new WorldRecordLoadedEventArgs
        {
            Record = newRecord
        });
    }
}
