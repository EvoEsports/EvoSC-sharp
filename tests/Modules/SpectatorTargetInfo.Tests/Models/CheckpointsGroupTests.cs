﻿using EvoSC.Common.Interfaces.Models.Enums;
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

        Assert.Equal(1, checkpointsGroup.GetPlayerCheckpointData(targetPlayer.GetLogin())?.time);

        return Task.CompletedTask;
    }

    [Fact]
    public Task Returns_Null_For_Non_Existent_Player_data()
    {
        var checkpointsGroup = new CheckpointsGroup();

        Assert.Null(checkpointsGroup.GetPlayerCheckpointData("*fakeplayer1*"));

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

        Assert.Equal(2, checkpointsGroup.GetRank(targetPlayer.AccountId));

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

        Assert.Equal(3, checkpointsGroup.GetRank(targetPlayer.AccountId));

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

        Assert.Equal(2, checkpointsGroup.GetRank(targetPlayer.AccountId));

        return Task.CompletedTask;
    }
}
