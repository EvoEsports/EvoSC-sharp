using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.RoundRankingModule.Controllers;
using EvoSC.Modules.Official.RoundRankingModule.Interfaces;
using EvoSC.Modules.Official.RoundRankingModule.Models;
using EvoSC.Testing.Controllers;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.RoundRankingModule.Tests.Controllers;

public class RoundRankingEventControllerTests : ControllerMock<RoundRankingEventController, IEventControllerContext>
{
    private Mock<IRoundRankingService> _roundRankingService = new();
    private Mock<IPlayerManagerService> _playerManagerService = new();

    public RoundRankingEventControllerTests()
    {
        InitMock(_roundRankingService, _playerManagerService);
    }

    [Fact]
    public async Task Consumes_Checkpoint_Data()
    {
        var player = new OnlinePlayer { Id = 0, State = PlayerState.Playing, AccountId = "*fakeplayer1*" };

        _playerManagerService.Setup(pms => pms.GetOnlinePlayerAsync(player.AccountId))
            .ReturnsAsync(player);

        var waypointEventArgs = new WayPointEventArgs
        {
            Login = player.GetLogin(),
            AccountId = player.AccountId,
            RaceTime = 1234,
            LapTime = 1234,
            CheckpointInRace = 1,
            CheckpointInLap = 1,
            IsEndRace = false,
            IsEndLap = false,
            CurrentRaceCheckpoints = [],
            CurrentLapCheckpoints = [],
            BlockId = "",
            Speed = 0,
            Time = 0
        };

        var checkpointData = CheckpointData.FromWaypointEventArgs(player, waypointEventArgs);
        
        Assert.Equal(player, checkpointData.Player);
        
        await Controller.OnWaypointAsync(null, waypointEventArgs);

        _roundRankingService.Verify(rrs => rrs.ConsumeCheckpointDataAsync(It.IsAny<CheckpointData>()));
    }
}
