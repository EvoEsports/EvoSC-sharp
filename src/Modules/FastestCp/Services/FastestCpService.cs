using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.FastestCp.Interfaces;
using EvoSC.Modules.Official.FastestCp.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.FastestCp.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class FastestCpService : IFastestCpService
{
    private readonly ILogger<FastestCpService> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IManialinkManager _manialinkManager;
    private readonly IPlayerManagerService _playerManagerService;

    private FastestCpStore _fastestCpStore;

    public FastestCpService(IPlayerManagerService playerManagerService, IManialinkManager manialinkManager,
        ILoggerFactory loggerFactory, ILogger<FastestCpService> logger)
    {
        _playerManagerService = playerManagerService;
        _logger = logger;
        _loggerFactory = loggerFactory;
        _manialinkManager = manialinkManager;
        _fastestCpStore = GetNewFastestCpStore();
    }

    public async Task RegisterCpTime(WayPointEventArgs args)
    {
        var result = _fastestCpStore.RegisterTime(args.AccountId, args.CheckpointInRace, args.RaceTime, args.Time);
        await _manialinkManager.SendManialinkAsync("FastestCp.FastestCp", new { data = await GetCurrentBestCpTimes() });
        if (result)
        {
            // TODO update manialinks
        }
    }

    public async Task<PlayerCpTime?[]> GetCurrentBestCpTimes()
    {
        var playerCache = new Dictionary<string, IPlayer>();
        return await Task.WhenAll(_fastestCpStore.GetFastestTimes().Select(async time =>
        {
            if (time == null)
            {
                return null;
            }

            if (playerCache.TryGetValue(time.AccountId, out var player))
            {
                return new PlayerCpTime(player, time.RaceTime);
            }

            player = await _playerManagerService.GetOrCreatePlayerAsync(time.AccountId);
            playerCache.Add(time.AccountId, player);
            return new PlayerCpTime(player, time.RaceTime);
        }));
    }

    public async Task ResetCpTimes()
    {
        _fastestCpStore = GetNewFastestCpStore();
        await _manialinkManager.SendManialinkAsync("FastestCp.FastestCp", new { data = await GetCurrentBestCpTimes() });
    }

    private FastestCpStore GetNewFastestCpStore() => new(_loggerFactory.CreateLogger<FastestCpStore>());
}
