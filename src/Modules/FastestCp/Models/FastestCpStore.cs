using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.FastestCp.Models;

internal class FastestCpStore
{
    private readonly Dictionary<AccountIdCpNumber, ServerCpTime> _cache = new();
    private readonly List<SortedList<ServerCpTime, AccountIdCpTime>?> _fastestTimes = new();
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

        lock (_cache)
        {
            var oldCpTime = _cache.GetValueOrDefault(accountIdCp);
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
                    "Extending fastest checkpoint list from {oldSize} to {newSize} to insert first time driven ({cpTime}) at checkpoint {cpIndex}",
                    _fastestTimes.Count, cpIndex + 1, cpTime, cpIndex);
                _fastestTimes.AddRange(
                    new SortedList<ServerCpTime, AccountIdCpTime>[cpIndex - _fastestTimes.Count + 1]);
                _fastestTimes[cpIndex] =
                    new SortedList<ServerCpTime, AccountIdCpTime> { { newCpTime, accountIdCpTime } };
            }
            else if (_fastestTimes[cpIndex] == null)
            {
                _logger.LogDebug(
                    "Inserting first checkpoint time ({cpTime}) into fastest checkpoint list driven at checkpoint {cpIndex}",
                    cpTime, cpIndex);
                _fastestTimes[cpIndex] =
                    new SortedList<ServerCpTime, AccountIdCpTime> { { newCpTime, accountIdCpTime } };
            }
            else
            {
                _logger.LogDebug(
                    "Update fastest checkpoint time ({cpTime}) in sorted checkpoint time list at checkpoint {cpIndex} driven by account {accountId}",
                    cpTime, cpIndex, accountId);
                _fastestTimes[cpIndex]![newCpTime] = accountIdCpTime;
            }

            return true;
        }
    }

    public List<List<AccountIdCpTime>> GetFastestTimes(int limit)
    {
        lock (_fastestTimes)
        {
            return _fastestTimes.Select(sl => sl?.Values.Take(limit).ToList() ?? new List<AccountIdCpTime>()).ToList();
        }
    }
}
