using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.FastestCp.Models;

public class FastestCpStore
{
    private readonly List<SortedList<ServerCpTime, AccountIdCpTime>?> _fastestTimes = new();
    private readonly Dictionary<AccountIdCpNumber, ServerCpTime> _cache = new();
    private readonly ILogger<FastestCpStore> _logger;

    public FastestCpStore(ILogger<FastestCpStore> logger)
    {
        _logger = logger;
    }

    public bool RegisterTime(string accountId, int cpIndex, int cpTime, int serverTime)
    {
        var accountIdCp = new AccountIdCpNumber(accountId, cpIndex);
        var accountIdCpTime = new AccountIdCpTime(accountId, cpTime);
        var newCpTime = new ServerCpTime(cpTime, serverTime);
        ServerCpTime? oldCpTime;

        lock (_cache)
        {
            oldCpTime = _cache.GetValueOrDefault(accountIdCp);
            if (oldCpTime != null && oldCpTime.RaceTime <= cpTime)
            {
                _logger.LogDebug(
                    "Not inserting slower checkpoint time of {cpTime} at checkpoint {cpIndex} driven by account {accountId} into the checkpoint times cache",
                    cpTime, cpIndex, accountId);
                return false;
            }

            _logger.LogDebug(
                "Inserting faster checkpoint time of {cpTime} at checkpoint {cpIndex} driven by account {accountId} into the checkpoint times cache",
                cpTime, cpIndex, accountId);
            _cache[accountIdCp] = newCpTime;
        }

        lock (_fastestTimes)
        {
            if (cpIndex >= _fastestTimes.Count)
            {
                _logger.LogDebug(
                    "Extending fastest checkpoint list from {oldSize} to {newSize} to insert first time driven at checkpoint {cpIndex}",
                    _fastestTimes.Count, cpIndex + 1, cpIndex);
                _fastestTimes.AddRange(
                    new SortedList<ServerCpTime, AccountIdCpTime>[cpIndex - _fastestTimes.Count + 1]);
                _fastestTimes[cpIndex] =
                    new SortedList<ServerCpTime, AccountIdCpTime> { { newCpTime, accountIdCpTime } };
            }
            else if (_fastestTimes[cpIndex] == null)
            {
                _logger.LogDebug("Inserting first checkpoint time driven at checkpoint {cpIndex}", cpIndex);
                _fastestTimes[cpIndex] =
                    new SortedList<ServerCpTime, AccountIdCpTime> { { newCpTime, accountIdCpTime } };
            }
            else
            {
                _logger.LogDebug(
                    "Update fastest checkpoint time in sorted checkpoint time list at checkpoint {cpTime} driven by account {accountId}",
                    cpTime, accountId);
                var fastestTimesForCp = _fastestTimes[cpIndex]!;
                if (oldCpTime != null)
                {
                    fastestTimesForCp.Remove(oldCpTime);
                }

                fastestTimesForCp.Add(newCpTime, accountIdCpTime);
            }

            return true;
        }
    }

    public List<List<AccountIdCpTime>> GetFastestTimes(int limit)
    {
        lock (_fastestTimes)
        {
            return _fastestTimes.Select(sl => sl?.Values.Take(limit).ToList() ?? new()).ToList();
        }
    }
}
