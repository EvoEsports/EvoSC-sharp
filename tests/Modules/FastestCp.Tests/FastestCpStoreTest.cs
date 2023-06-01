using EvoSC.Modules.Official.FastestCp.Models;
using Microsoft.Extensions.Logging.Abstractions;

namespace EvoSC.Modules.Official.FastestCp.Tests;

public class FastestCpStoreTest
{
    private readonly FastestCpStore _fastestCpStore = new(new NullLogger<FastestCpStore>());

    public static IEnumerable<object[]> GetTestData()
    {
        yield return new object[] { new List<TestEntry>(), new List<bool>(), new List<AccountIdCpTime?>() };
        yield return new object[]
        {
            new List<TestEntry> { new("acc1", 0, 10) }, new List<bool> { true },
            new List<AccountIdCpTime?> { new("acc1", 10) }
        };
        yield return new object[]
        {
            new List<TestEntry> { new("acc1", 0, 10), new("acc2", 0, 5) }, new List<bool> { true, true },
            new List<AccountIdCpTime?> { new("acc2", 5) }
        };
        yield return new object[]
        {
            new List<TestEntry> { new("acc1", 0, 10), new("acc2", 0, 15) }, new List<bool> { true, false },
            new List<AccountIdCpTime?> { new("acc1", 10) }
        };
        yield return new object[]
        {
            new List<TestEntry> { new("acc1", 0, 10), new("acc2", 0, 10) }, new List<bool> { true, false },
            new List<AccountIdCpTime?> { new("acc1", 10) }
        };
        yield return new object[]
        {
            new List<TestEntry> { new("acc1", 0, 10), new("acc2", 0, 10) }, new List<bool> { true, false },
            new List<AccountIdCpTime?> { new("acc1", 10) }
        };
        yield return new object[]
        {
            new List<TestEntry> { new("acc1", 0, 10), new("acc2", 1, 10) }, new List<bool> { true, true },
            new List<AccountIdCpTime?> { new("acc1", 10), new("acc2", 10) }
        };
        yield return new object[]
        {
            new List<TestEntry> { new("acc1", 1, 10), new("acc2", 0, 10) }, new List<bool> { true, true },
            new List<AccountIdCpTime?> { new("acc2", 10), new("acc1", 10) }
        };
        yield return new object[]
        {
            new List<TestEntry> { new("acc1", 1, 10) }, new List<bool> { true },
            new List<AccountIdCpTime?> { null, new("acc1", 10) }
        };
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void Inserted_Records_Should_Return_Correct_Result(List<TestEntry> times, List<bool> results,
        List<AccountIdCpTime?> expected)
    {
        var actual = times.Select(entry => _fastestCpStore.RegisterTime(entry.Account, entry.Index, entry.Race))
            .ToList();
        var data = _fastestCpStore.GetFastestTimes();

        Assert.Equivalent(results, actual);
        Assert.Equivalent(expected, data);
    }

    public record TestEntry(string Account, int Index, int Race);
}
