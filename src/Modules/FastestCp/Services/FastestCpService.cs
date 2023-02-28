using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.FastestCp.Interfaces;
using EvoSC.Modules.Official.FastestCp.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.FastestCp.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class FastestCpService : IFastestCpService
{
    private readonly ILogger<FastestCpService> _logger;
    private readonly LoggerFactory _loggerFactory;
    private readonly IPlayerManagerService _playerManagerService;

    private FastestCpStore _fastestCpStore;

    public FastestCpService(IPlayerManagerService playerManagerService, ILogger<FastestCpService> logger, LoggerFactory loggerFactory)
    {
        _playerManagerService = playerManagerService;
        _logger = logger;
        _loggerFactory = loggerFactory;
        _fastestCpStore = GetNewFastestCpStore();
    }

    public void RegisterCpTime(WayPointEventArgs args)
    {
        var result = _fastestCpStore.RegisterTime(args.AccountId, args.CheckpointInRace, args.RaceTime, args.Time);
        if (result)
        {
            // TODO update manialinks
        }
    }

    public async Task<PlayerCpTime[][]> GetCurrentBestCpTimes(int limit)
    {
        var playerCache = new Dictionary<string, IPlayer>();
        return await Task.WhenAll(_fastestCpStore.GetFastestTimes(limit).Select(l => Task.WhenAll(l.Select(async time =>
        {
            if (playerCache.TryGetValue(time.AccountId, out var player))
            {
                return new PlayerCpTime(player, time.RaceTime);
            }

            player = await _playerManagerService.GetOrCreatePlayerAsync(time.AccountId);
            playerCache.Add(time.AccountId, player);
            return new PlayerCpTime(player, time.RaceTime);
        }))));
    }

    public void ResetCpTimes()
    {
        _fastestCpStore = GetNewFastestCpStore();
    }

    private FastestCpStore GetNewFastestCpStore()
    {
        return new FastestCpStore(_loggerFactory.CreateLogger<FastestCpStore>());
    }
}
