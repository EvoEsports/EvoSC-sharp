namespace EvoSC.Modules.Official.FastestCp.Models;

public class FastestCpStore
{
    private readonly List<SortedList<ServerCpTime, AccountIdCpTime>?> _fastestTimes = new();
    private readonly Dictionary<AccountIdCpNumber, ServerCpTime> _timesDriven = new();

    public bool RegisterTime(string accountId, int cpIndex, int cpTime, int serverTime)
    {
        var accountIdCp = new AccountIdCpNumber(accountId, cpIndex);
        var accountIdCpTime = new AccountIdCpTime(accountId, cpTime);
        var newCpTime = new ServerCpTime(cpTime, serverTime);
        ServerCpTime? oldCpTime;

        lock (_timesDriven)
        {
            oldCpTime = _timesDriven.GetValueOrDefault(accountIdCp);
            if (oldCpTime != null && oldCpTime.RaceTime <= cpTime)
            {
                return false;
            }

            _timesDriven[accountIdCp] = newCpTime;
        }

        lock (_fastestTimes)
        {
            if (cpIndex >= _fastestTimes.Count)
            {
                _fastestTimes.AddRange(
                    new SortedList<ServerCpTime, AccountIdCpTime>[cpIndex - _fastestTimes.Count + 1]);
                _fastestTimes[cpIndex] =
                    new SortedList<ServerCpTime, AccountIdCpTime> { { newCpTime, accountIdCpTime } };
            }
            else if (_fastestTimes[cpIndex] == null)
            {
                _fastestTimes[cpIndex] =
                    new SortedList<ServerCpTime, AccountIdCpTime> { { newCpTime, accountIdCpTime } };
            }
            else
            {
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
