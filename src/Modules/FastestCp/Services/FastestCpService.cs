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
    private readonly IPlayerManagerService _playerManagerService;

    private FastestCpStore _fastestCpStore = new();

    public FastestCpService(IPlayerManagerService playerManagerService, ILogger<FastestCpService> logger)
    {
        _playerManagerService = playerManagerService;
        _logger = logger;
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
        return await Task.WhenAll(_fastestCpStore.GetFastestTimes(limit).Select(l => Task.WhenAll(l.Select(async time =>
            new PlayerCpTime(await _playerManagerService.GetOrCreatePlayerAsync(time.AccountId), time.RaceTime)))));
    }

    public void ResetCpTimes()
    {
        _fastestCpStore = new FastestCpStore();
    }
}
