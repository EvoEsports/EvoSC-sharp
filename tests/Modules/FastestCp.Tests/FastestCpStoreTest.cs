using EvoSC.Modules.Official.FastestCp.Models;
using Microsoft.Extensions.Logging.Abstractions;

namespace EvoSC.Modules.Official.FastestCp.Tests;

public class FastestCpStoreTest
{
    private readonly FastestCpStore _fastestCpStore = new(new NullLogger<FastestCpStore>());

    [Fact]
    public void Empty_Records_Should_Be_Empty()
    {
        var result = _fastestCpStore.GetFastestTimes();

        Assert.Empty(result);
    }

    [Fact]
    public void Single_Record_Should_Be_Inserted()
    {
        const string Account = "AccountId";
        const int RaceTime = 10;

        var result = _fastestCpStore.RegisterTime(Account, 0, RaceTime);

        var data = _fastestCpStore.GetFastestTimes();

        Assert.True(result);
        Assert.Equivalent(new List<AccountIdCpTime?>
        {
            new(Account, RaceTime)
        }, data);
    }

    [Fact]
    public void Faster_Record_Should_Be_Inserted()
    {
        const string Account = "AccountId";
        const int RaceTime1 = 10;
        const int RaceTime2 = 5;

        _fastestCpStore.RegisterTime(Account, 0, RaceTime1);

        var result = _fastestCpStore.RegisterTime(Account, 0, RaceTime2);

        var data = _fastestCpStore.GetFastestTimes();

        Assert.True(result);
        Assert.Equivalent(new List<AccountIdCpTime?>
        {
            new(Account, RaceTime2)
        }, data);
    }

    [Fact]
    public void Slower_Record_Should_Not_Be_Inserted()
    {
        const string Account = "AccountId";
        const int RaceTime1 = 10;
        const int RaceTime2 = 12;

        _fastestCpStore.RegisterTime(Account, 0, RaceTime1);

        var result = _fastestCpStore.RegisterTime(Account, 0, RaceTime2);

        var data = _fastestCpStore.GetFastestTimes();

        Assert.False(result);
        Assert.Equivalent(new List<AccountIdCpTime?>
        {
            new(Account, RaceTime1)
        }, data);
    }

    [Fact]
    public void Same_Record_Should_Not_Be_Inserted()
    {
        const string Account = "AccountId";
        const int RaceTime1 = 10;
        const int RaceTime2 = 10;

        _fastestCpStore.RegisterTime(Account, 0, RaceTime1);

        var result = _fastestCpStore.RegisterTime(Account, 0, RaceTime2);

        var data = _fastestCpStore.GetFastestTimes();

        Assert.False(result);
        Assert.Equivalent(new List<AccountIdCpTime?>
        {
            new(Account, RaceTime1)
        }, data);
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

        _fastestCpStore.RegisterTime(Account1, 0, RaceTime1);

        var result2 = _fastestCpStore.RegisterTime(Account2, 0, RaceTime2);
        var data2 = _fastestCpStore.GetFastestTimes();
        var result3 = _fastestCpStore.RegisterTime(Account3, 0, RaceTime3);
        var data3 = _fastestCpStore.GetFastestTimes();

        Assert.False(result2);
        Assert.True(result3);
        Assert.Equivalent(
            new List<AccountIdCpTime>
            {
                new(Account1, RaceTime1)
            }, data2);
        Assert.Equivalent(
            new List<AccountIdCpTime?>
            {
                new(Account3, RaceTime3),
            }, data3);
    }

    [Fact]
    public void Multiple_Records_Should_Be_Inserted_At_Different_Cps()
    {
        const string Account = "AccountId";
        const int RaceTime1 = 10;
        const int RaceTime2 = 20;

        _fastestCpStore.RegisterTime(Account, 0, RaceTime1);
        var result2 = _fastestCpStore.RegisterTime(Account, 1, RaceTime2);

        var data = _fastestCpStore.GetFastestTimes();

        Assert.True(result2);
        Assert.Equivalent(
            new List<AccountIdCpTime?>
            {
                new(Account, RaceTime1), 
                new(Account, RaceTime2)
            },
            data);
    }

    [Fact]
    public void Multiple_Records_Should_Be_Inserted_At_Different_Cps_In_Wrong_Order()
    {
        const string Account = "AccountId";
        const int RaceTime1 = 10;
        const int RaceTime2 = 20;

        _fastestCpStore.RegisterTime(Account, 1, RaceTime2);
        var result1 = _fastestCpStore.RegisterTime(Account, 0, RaceTime1);

        var data = _fastestCpStore.GetFastestTimes();

        Assert.True(result1);
        Assert.Equivalent(
            new List<AccountIdCpTime?>
            {
                new(Account, RaceTime1), 
                new(Account, RaceTime2) 
            },
            data);
    }

    [Fact]
    public void Single_Record_Should_Be_Inserted_At_Different_Cps()
    {
        const string Account = "AccountId";
        const int RaceTime = 10;

        var result = _fastestCpStore.RegisterTime(Account, 1, RaceTime);

        var data = _fastestCpStore.GetFastestTimes();

        Assert.True(result);
        Assert.Equivalent(new List<AccountIdCpTime?>
            {
                null, 
                new(Account, RaceTime)
            },
            data);
    }
}
