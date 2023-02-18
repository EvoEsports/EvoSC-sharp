using System.Collections.Concurrent;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.FastestCp.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.FastestCp.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class FastestCpService : IFastestCpService
{
    private readonly IPlayerManagerService _playerManagerService;
    private readonly ILogger<FastestCpService> _logger;

    private List<ConcurrentDictionary<string, CpTime>?> _cpTimes = new();

    public FastestCpService(IPlayerManagerService playerManagerService, ILogger<FastestCpService> logger)
    {
        _playerManagerService = playerManagerService;
        _logger = logger;
    }

    public void RegisterCpTime(WayPointEventArgs args)
    {
        if (args.CheckpointInRace >= _cpTimes.Count)
        {
            _logger.LogDebug("Current fastest checkpoint list too small. Extending list from {before} to {after} checkpoints.", _cpTimes.Count - 1, args.CheckpointInRace + 1);
            _cpTimes.AddRange(new ConcurrentDictionary<string, CpTime>[args.CheckpointInRace - _cpTimes.Count + 1]);
        }

        var newTime = new CpTime(args.RaceTime, args.Time);
        var currentValue = _cpTimes.ElementAtOrDefault(args.CheckpointInRace);
        if (currentValue == null)
        {
            _logger.LogDebug("Current fastest checkpoints empty for checkpoint {currentCp}. Inserting new dictionary of checkpoint times at index {index}", args.CheckpointInRace, args.CheckpointInRace);
            _logger.LogDebug("Inserted new fastest checkpoint time {raceTime} driven by account {accountId} at {time} for checkpoint {currentCp}.", args.RaceTime, args.AccountId, args.Time, args.CheckpointInRace);
            _cpTimes[args.CheckpointInRace] = new ConcurrentDictionary<string, CpTime> { [args.AccountId] = newTime };
            return;
        }

        var result = currentValue.AddOrUpdate(args.AccountId, newTime, (_, current) => CpTime.Min(current, newTime));
        if (result == newTime)
        {
            _logger.LogDebug("Inserted new fastest checkpoint time {raceTime} driven by account {accountId} at {time} for checkpoint {currentCp}.", args.RaceTime, args.AccountId, args.Time, args.CheckpointInRace);
        }
    }

    public IEnumerable<IEnumerable<Task<FastestCpTime>>> GetCurrentBestCpTimes(string accountId, int limit)
    {
        _logger.LogDebug("Requesting fastest {limit} checkpoints for account {id}.", limit, accountId);
        return _cpTimes
            .ConvertAll(dict =>
                dict?.OrderBy(x => x.Value, CpTime.Comparator)
                    .Where((entry, i) => i < limit || entry.Key == accountId)
                    .Select(async pair => new FastestCpTime(await _playerManagerService.GetOnlinePlayerAsync(pair.Key),
                        pair.Value.RaceTime))
                ?? Enumerable.Empty<Task<FastestCpTime>>());
    }

    public void ResetCpTimes()
    {
        _logger.LogDebug("Delete current fastest checkpoints due to map switch.");
        _cpTimes = new();
    }
}
