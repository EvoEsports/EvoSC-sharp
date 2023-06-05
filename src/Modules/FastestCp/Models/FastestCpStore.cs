using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.FastestCp.Models;

internal class FastestCpStore
{
    private readonly List<AccountIdCpTime?> _fastestTimes = new();
    private readonly object _listMutex = new();
    private readonly ILogger<FastestCpStore> _logger;

    internal FastestCpStore(ILogger<FastestCpStore> logger)
    {
        _logger = logger;
    }

    internal bool RegisterTime(string accountId, int cpIndex, int cpTime)
    {
        var accountIdCpTime = new AccountIdCpTime(accountId, cpTime);

        lock (_listMutex)
        {
            if (cpIndex >= _fastestTimes.Count)
            {
                _logger.LogTrace(
                    "Extending fastest checkpoint list from {OldSize} to {NewSize} to insert first time driven ({CpTime}) at checkpoint {CpIndex}",
                    _fastestTimes.Count, cpIndex + 1, cpTime, cpIndex);
                _fastestTimes.AddRange(
                    new AccountIdCpTime[cpIndex - _fastestTimes.Count + 1]);
                _fastestTimes[cpIndex] = new AccountIdCpTime(accountId, cpTime);
                return true;
            }

            if (_fastestTimes[cpIndex] == null)
            {
                _logger.LogTrace(
                    "Inserting first checkpoint time ({CpTime}) at checkpoint {CpIndex} driven by account {AccountId}",
                    cpTime, cpIndex, accountId);
                _fastestTimes[cpIndex] = new AccountIdCpTime(accountId, cpTime);
                return true;
            }

            if (_fastestTimes[cpIndex]!.RaceTime > cpTime)
            {
                _logger.LogTrace(
                    "Update fastest checkpoint time ({CpTime}) at checkpoint {CpIndex} driven by account {AccountId}",
                    cpTime, cpIndex, accountId);
                _fastestTimes[cpIndex] = accountIdCpTime;
                return true;
            }

            _logger.LogTrace(
                "Do not update slower checkpoint time ({CpTime}) at checkpoint {CpIndex} driven by account {AccountId}",
                cpTime, cpIndex, accountId);
            return false;
        }
    }

    internal List<AccountIdCpTime?> GetFastestTimes()
    {
        return _fastestTimes.ToList();
    }
}
