using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Models;
using Xunit;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Tests.Models;

public class CheckpointsGroupTests
{
    [Fact]
    public Task Gets_Player_Data_From_Group()
    {
        var checkpointsGroup = new CheckpointsGroup();
        var targetPlayer = new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer1*" };

        checkpointsGroup.Add(
            new CheckpointData(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer10*" }, 0));
        checkpointsGroup.Add(new CheckpointData(targetPlayer, 1));
        checkpointsGroup.Add(
            new CheckpointData(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer12*" }, 2));

        var time = checkpointsGroup.GetPlayerCheckpointData(targetPlayer.GetLogin())?.time;

        Assert.Equal(1, time);

        return Task.CompletedTask;
    }

    [Fact]
    public Task Returns_Null_For_Non_Existent_Player_data()
    {
        var checkpointsGroup = new CheckpointsGroup();
        var checkpointData = checkpointsGroup.GetPlayerCheckpointData("*fakeplayer1*");

        Assert.Null(checkpointData);

        return Task.CompletedTask;
    }

    [Fact]
    public Task Gets_Rank_Of_Player()
    {
        var checkpointsGroup = new CheckpointsGroup();
        var targetPlayer = new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer1*" };

        checkpointsGroup.Add(
            new CheckpointData(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer10*" }, 0));
        checkpointsGroup.Add(new CheckpointData(targetPlayer, 1));
        checkpointsGroup.Add(
            new CheckpointData(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer12*" }, 2));

        var rank = checkpointsGroup.GetRank(targetPlayer.AccountId);

        Assert.Equal(2, rank);

        return Task.CompletedTask;
    }

    [Fact]
    public Task Gets_Rank_Of_Player_Correctly_If_Another_Player_Has_The_Same_Time_Before()
    {
        var checkpointsGroup = new CheckpointsGroup();
        var targetPlayer = new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer1*" };

        checkpointsGroup.Add(
            new CheckpointData(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer10*" }, 0));
        checkpointsGroup.Add(
            new CheckpointData(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer11*" }, 1));
        checkpointsGroup.Add(new CheckpointData(targetPlayer, 1));
        checkpointsGroup.Add(
            new CheckpointData(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer12*" }, 2));

        var rank = checkpointsGroup.GetRank(targetPlayer.AccountId);

        Assert.Equal(3, rank);

        return Task.CompletedTask;
    }

    [Fact]
    public Task Gets_Rank_Of_Player_Correctly_If_Another_Player_Has_The_Same_Time_After()
    {
        var checkpointsGroup = new CheckpointsGroup();
        var targetPlayer = new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer1*" };

        checkpointsGroup.Add(
            new CheckpointData(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer10*" }, 0));
        checkpointsGroup.Add(new CheckpointData(targetPlayer, 1));
        checkpointsGroup.Add(
            new CheckpointData(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer11*" }, 1));
        checkpointsGroup.Add(
            new CheckpointData(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer12*" }, 2));

        var rank = checkpointsGroup.GetRank(targetPlayer.AccountId);

        Assert.Equal(2, rank);

        return Task.CompletedTask;
    }

    [Fact]
    public Task Forgets_Given_Player()
    {
        var checkpointsGroup = new CheckpointsGroup();
        var targetPlayer = new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer1*" };

        checkpointsGroup.Add(new CheckpointData(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer10*" }, 0));
        checkpointsGroup.Add(new CheckpointData(targetPlayer, 1));
        checkpointsGroup.Add(new CheckpointData(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer11*" }, 1));
        checkpointsGroup.Add(new CheckpointData(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer12*" }, 2));

        var entryRemoved = checkpointsGroup.ForgetPlayer(targetPlayer.GetLogin());
        
        Assert.True(entryRemoved);

        return Task.CompletedTask;
    }

    [Fact]
    public Task Does_Not_Forget_Non_Existent_Player()
    {
        var checkpointsGroup = new CheckpointsGroup();
        var entryRemoved = checkpointsGroup.ForgetPlayer("*fakeplayer1*");
        
        Assert.False(entryRemoved);

        return Task.CompletedTask;
    }
}
