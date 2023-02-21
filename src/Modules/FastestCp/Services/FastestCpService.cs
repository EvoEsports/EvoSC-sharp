using System.Collections.Concurrent;
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
    private readonly IPlayerManagerService _playerManagerService;
    private readonly ILogger<FastestCpService> _logger;

    private ConcurrentDictionary<int, SortedList<CpTime, AccountIdCpTime>> _cpTimes = new();

    public FastestCpService(IPlayerManagerService playerManagerService, ILogger<FastestCpService> logger)
    {
        _playerManagerService = playerManagerService;
        _logger = logger;
    }

    public void RegisterCpTime(WayPointEventArgs args)
    {
        var cpTime = new CpTime(args.RaceTime, args.Time);
        var accountIdCpTime = new AccountIdCpTime(args.AccountId, args.RaceTime);
        _cpTimes.AddOrUpdate(args.RaceTime, new SortedList<CpTime, AccountIdCpTime> { { cpTime, accountIdCpTime } },
            (_, list) =>
            {
                lock (list)
                {
                    if (args.RaceTime < list[cpTime].RaceTime)
                    {
                        list[cpTime] = accountIdCpTime;
                        _logger.LogDebug(
                            "Inserted new fastest checkpoint time {raceTime} driven by account {accountId} at {time} for checkpoint {currentCp}.",
                            args.RaceTime, args.AccountId, args.Time, args.CheckpointInRace);
                    }
                }

                return list;
            });
    }

    public async Task<PlayerCpTime[][]> GetCurrentBestCpTimes(int limit)
    {
        return await Task.WhenAll(_cpTimes.Values.Select(list => Task.WhenAll(list.Values.Take(limit)
            .Select(async time =>
                new PlayerCpTime(await _playerManagerService.GetOnlinePlayerAsync(time.AccountId), time.RaceTime)))));
    }

    public void ResetCpTimes()
    {
        lock (_cpTimes)
        {
            _logger.LogDebug("Delete current fastest checkpoints due to map switch.");
            _cpTimes = new();
        }
    }
}
