using EvoSC.Modules.Official.FastestCp.Models;
using Microsoft.Extensions.Logging.Abstractions;

namespace EvoSC.Modules.Official.FastestCp.Tests;

public class FastestCpStoreTest
{
    private readonly FastestCpStore _fastestCpStore = new(new NullLogger<FastestCpStore>());

    [Fact]
    public void Empty_Records_Should_Be_Empty()
    {
        var result = _fastestCpStore.GetFastestTimes(3);

        Assert.Empty(result);
    }

    [Fact]
    public void Single_Record_Should_Be_Inserted()
    {
        const string Account = "AccountId";
        const int RaceTime = 10;

        var result = _fastestCpStore.RegisterTime(Account, 0, RaceTime, 100);

        var data = _fastestCpStore.GetFastestTimes(3);

        Assert.True(result);
        Assert.Equivalent(new List<List<AccountIdCpTime>> { new() { new AccountIdCpTime(Account, RaceTime) } }, data);
    }

    [Fact]
    public void Faster_Record_Should_Be_Inserted()
    {
        const string Account = "AccountId";
        const int RaceTime1 = 10;
        const int RaceTime2 = 5;
        const int ServerTime1 = 100;
        const int ServerTime2 = 200;

        _fastestCpStore.RegisterTime(Account, 0, RaceTime1, ServerTime1);

        var result = _fastestCpStore.RegisterTime(Account, 0, RaceTime2, ServerTime2);

        var data = _fastestCpStore.GetFastestTimes(3);

        Assert.True(result);
        Assert.Equivalent(new List<List<AccountIdCpTime>> { new() { new AccountIdCpTime(Account, RaceTime2) } }, data);
    }

    [Fact]
    public void Slower_Record_Should_Not_Be_Inserted()
    {
        const string Account = "AccountId";
        const int RaceTime1 = 10;
        const int RaceTime2 = 12;
        const int ServerTime1 = 100;
        const int ServerTime2 = 200;

        _fastestCpStore.RegisterTime(Account, 0, RaceTime1, ServerTime1);

        var result = _fastestCpStore.RegisterTime(Account, 0, RaceTime2, ServerTime2);

        var data = _fastestCpStore.GetFastestTimes(3);

        Assert.False(result);
        Assert.Equivalent(new List<List<AccountIdCpTime>> { new() { new AccountIdCpTime(Account, RaceTime1) } }, data);
    }

    [Fact]
    public void Same_Record_Should_Not_Be_Inserted()
    {
        const string Account = "AccountId";
        const int RaceTime1 = 10;
        const int RaceTime2 = 10;
        const int ServerTime1 = 100;
        const int ServerTime2 = 200;

        _fastestCpStore.RegisterTime(Account, 0, RaceTime1, ServerTime1);

        var result = _fastestCpStore.RegisterTime(Account, 0, RaceTime2, ServerTime2);

        var data = _fastestCpStore.GetFastestTimes(3);

        Assert.False(result);
        Assert.Equivalent(new List<List<AccountIdCpTime>> { new() { new AccountIdCpTime(Account, RaceTime1) } }, data);
    }

    [Fact]
    public void Multiple_Records_Should_Be_Inserted_In_Correct_Order()
    {
        const string Account1 = "AccountId1";
        const string Account2 = "AccountId2";
        const string Account3 = "AccountId3";
        const int RaceTime1 = 10;
        const int RaceTime2 = 20;
        const int RaceTime3 = 5;
        const int ServerTime1 = 100;
        const int ServerTime2 = 200;
        const int ServerTime3 = 300;

        _fastestCpStore.RegisterTime(Account1, 0, RaceTime1, ServerTime1);

        var result2 = _fastestCpStore.RegisterTime(Account2, 0, RaceTime2, ServerTime2);
        var data2 = _fastestCpStore.GetFastestTimes(3);
        var result3 = _fastestCpStore.RegisterTime(Account3, 0, RaceTime3, ServerTime3);
        var data3 = _fastestCpStore.GetFastestTimes(3);

        Assert.True(result2);
        Assert.True(result3);
        Assert.Equivalent(
            new List<List<AccountIdCpTime>>
            {
                new() { new AccountIdCpTime(Account1, RaceTime1), new AccountIdCpTime(Account2, RaceTime2) }
            }, data2);
        Assert.Equivalent(
            new List<List<AccountIdCpTime>>
            {
                new()
                {
                    new AccountIdCpTime(Account3, RaceTime3),
                    new AccountIdCpTime(Account1, RaceTime1),
                    new AccountIdCpTime(Account2, RaceTime2)
                }
            }, data3);
    }

    [Fact]
    public void Multiple_Records_Should_Be_Limited()
    {
        const string Account1 = "AccountId1";
        const string Account2 = "AccountId2";
        const string Account3 = "AccountId3";
        const int RaceTime1 = 10;
        const int RaceTime2 = 20;
        const int RaceTime3 = 5;
        const int ServerTime1 = 100;
        const int ServerTime2 = 200;
        const int ServerTime3 = 300;

        _fastestCpStore.RegisterTime(Account1, 0, RaceTime1, ServerTime1);

        var result2 = _fastestCpStore.RegisterTime(Account2, 0, RaceTime2, ServerTime2);
        var data2 = _fastestCpStore.GetFastestTimes(2);
        var result3 = _fastestCpStore.RegisterTime(Account3, 0, RaceTime3, ServerTime3);
        var data3 = _fastestCpStore.GetFastestTimes(2);


        Assert.True(result2);
        Assert.True(result3);
        Assert.Equivalent(
            new List<List<AccountIdCpTime>>
            {
                new() { new AccountIdCpTime(Account1, RaceTime1), new AccountIdCpTime(Account2, RaceTime2) }
            }, data2);
        Assert.Equivalent(
            new List<List<AccountIdCpTime>>
            {
                new() { new AccountIdCpTime(Account3, RaceTime3), new AccountIdCpTime(Account1, RaceTime1) }
            }, data3);
    }

    [Fact]
    public void Multiple_Records_Should_Be_Inserted_At_Different_Cps()
    {
        const string Account = "AccountId";
        const int RaceTime1 = 10;
        const int RaceTime2 = 20;
        const int ServerTime1 = 100;
        const int ServerTime2 = 200;

        _fastestCpStore.RegisterTime(Account, 0, RaceTime1, ServerTime1);
        var result2 = _fastestCpStore.RegisterTime(Account, 1, RaceTime2, ServerTime2);

        var data = _fastestCpStore.GetFastestTimes(3);

        Assert.True(result2);
        Assert.Equivalent(
            new List<List<AccountIdCpTime>>
            {
                new() { new AccountIdCpTime(Account, RaceTime1) }, new() { new AccountIdCpTime(Account, RaceTime2) }
            },
            data);
    }

    [Fact]
    public void Multiple_Records_Should_Be_Inserted_At_Different_Cps_In_Wrong_Order()
    {
        const string Account = "AccountId";
        const int RaceTime1 = 10;
        const int RaceTime2 = 20;
        const int ServerTime1 = 100;
        const int ServerTime2 = 200;

        _fastestCpStore.RegisterTime(Account, 1, RaceTime2, ServerTime2);
        var result1 = _fastestCpStore.RegisterTime(Account, 0, RaceTime1, ServerTime1);

        var data = _fastestCpStore.GetFastestTimes(3);

        Assert.True(result1);
        Assert.Equivalent(
            new List<List<AccountIdCpTime>>
            {
                new() { new AccountIdCpTime(Account, RaceTime1) }, new() { new AccountIdCpTime(Account, RaceTime2) }
            },
            data);
    }

    [Fact]
    public void Single_Record_Should_Be_Inserted_At_Different_Cps()
    {
        const string Account = "AccountId";
        const int RaceTime = 10;

        var result = _fastestCpStore.RegisterTime(Account, 1, RaceTime, 100);

        var data = _fastestCpStore.GetFastestTimes(3);

        Assert.True(result);
        Assert.Equivalent(new List<List<AccountIdCpTime>> { new(), new() { new AccountIdCpTime(Account, RaceTime) } },
            data);
    }
}
