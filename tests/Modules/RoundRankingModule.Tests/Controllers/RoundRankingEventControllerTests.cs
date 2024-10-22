using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.RoundRankingModule.Controllers;
using EvoSC.Modules.Official.RoundRankingModule.Interfaces;
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
        Controller.OnWaypointAsync(null,
            new WayPointEventArgs
            {
                Login = "*fakeplayer1*",
                AccountId = "*fakeplayer1*",
                RaceTime = 0,
                LapTime = 0,
                CheckpointInRace = 0,
                CheckpointInLap = 0,
                IsEndRace = false,
                IsEndLap = false,
                CurrentRaceCheckpoints = [],
                CurrentLapCheckpoints = [],
                BlockId = "",
                Speed = 0,
                Time = 0
            });
    }
}
