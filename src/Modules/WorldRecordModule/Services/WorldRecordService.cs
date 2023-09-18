using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.LiveRankingModule.Models;
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
    private readonly IMapService _mapService;
    private WorldRecord? _currentWorldRecord;

    public WorldRecordService(ILogger<WorldRecordService> logger, IMapService mapService)
    {
        _logger = logger;
        _mapService = mapService;
    }

    public async Task FetchRecord(string mapUid)
    {
        TMioLeaderboardResponse res = await "https://trackmania.io"
            .AppendPathSegments("api", "leaderboard", "map", mapUid)
            .WithHeaders(new { User_Agent = "EvoSC# / World Record Grabber / Discord: chris92" })
            .GetJsonAsync<TMioLeaderboardResponse>();

        _logger.LogInformation("Loaded records for map.");

        if (res.tops.Count > 0)
        {
            var bestRecord = res.tops.First();
            var newWorldRecord = new WorldRecord
            {
                Name = bestRecord.player.name, Time = bestRecord.time, Source = "tm.io"
            };

            _logger.LogInformation("New best loaded from tm.io: {name} -> {time}", newWorldRecord.Name,
                newWorldRecord.Time);
            await OverwriteRecord(newWorldRecord);
        }
    }

    public Task OverwriteRecord(WorldRecord newRecord)
    {
        _currentWorldRecord = newRecord;

        //TODO: send NewWorldRecordLoaded event

        return Task.CompletedTask;
    }

    public Task ClearRecord()
    {
        _currentWorldRecord = null;

        return Task.CompletedTask;
    }

    public Task<WorldRecord?> GetRecord()
    {
        return Task.FromResult(_currentWorldRecord);
    }

    public Task DetectNewWorldRecordThroughScores(ScoresEventArgs scoresEventArgs)
    {
        if (_currentWorldRecord == null)
        {
            return Task.CompletedTask;
        }

        foreach (var score in scoresEventArgs.Players)
        {
            if (score != null && score.BestRaceTime < _currentWorldRecord.Time)
            {
                OverwriteRecord(new WorldRecord { Name = score.Name, Time = score.BestRaceTime, Source = "local" });
            }
        }

        return Task.CompletedTask;
    }
}
