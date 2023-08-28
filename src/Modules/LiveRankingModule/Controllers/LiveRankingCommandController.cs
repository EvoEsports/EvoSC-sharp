using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.LiveRankingModule.Interfaces;

namespace EvoSC.Modules.Official.LiveRankingModule.Controllers;

[Controller]
public class LiveRankingCommandController : EvoScController<ICommandInteractionContext>
{
    private readonly ILiveRankingService _liveRankingService;

    public LiveRankingCommandController(ILiveRankingService liveRankingService)
    {
        _liveRankingService = liveRankingService;
    }

    [ChatCommand("lr", "[Command.DebugLiveRanking]")]
    public async Task Debug()
    {
        await _liveRankingService.OnPlayerWaypointAsync(new WayPointEventArgs
        {
            AccountId = "39a38ee1-e0a0-49a0-93f5-8024cf1b7f9b",
            Login = "OaOO4eCgSaCT9YAkzxt_mw",
            RaceTime = 12000,
            LapTime = 12000,
            CheckpointInRace = 1,
            CheckpointInLap = 1,
            IsEndRace = false,
            IsEndLap = false,
            CurrentRaceCheckpoints = new List<int> { 1, 2, 3 },
            CurrentLapCheckpoints = new List<int> { 1, 2, 3 },
            BlockId = "bl",
            Speed = 100,
            Time = 12000
        });
        
        Thread.Sleep(1_000);
        
        await _liveRankingService.OnPlayerWaypointAsync(new WayPointEventArgs
        {
            AccountId = "085e1d24-7d55-496d-ad7a-1eb1efec09eb",
            Login = "CF4dJH1VSW2teh6x7-wJ6w",
            RaceTime = 14000,
            LapTime = 14000,
            CheckpointInRace = 1,
            CheckpointInLap = 1,
            IsEndRace = false,
            IsEndLap = false,
            CurrentRaceCheckpoints = new List<int> { 1, 2, 3 },
            CurrentLapCheckpoints = new List<int> { 1, 2, 3 },
            BlockId = "bl",
            Speed = 100,
            Time = 14000
        });
        
        Thread.Sleep(1_000);
        
        await _liveRankingService.OnPlayerWaypointAsync(new WayPointEventArgs
        {
            AccountId = "085e1d24-7d55-496d-ad7a-1eb1efec09eb",
            Login = "CF4dJH1VSW2teh6x7-wJ6w",
            RaceTime = 14888,
            LapTime = 14888,
            CheckpointInRace = 2,
            CheckpointInLap = 2,
            IsEndRace = false,
            IsEndLap = false,
            CurrentRaceCheckpoints = new List<int> { 1, 2, 3 },
            CurrentLapCheckpoints = new List<int> { 1, 2, 3 },
            BlockId = "bl",
            Speed = 100,
            Time = 14888
        });
        
        Thread.Sleep(100);
        
        await _liveRankingService.OnPlayerWaypointAsync(new WayPointEventArgs
        {
            AccountId = "39a38ee1-e0a0-49a0-93f5-8024cf1b7f9b",
            Login = "OaOO4eCgSaCT9YAkzxt_mw",
            RaceTime = 15000,
            LapTime = 15000,
            CheckpointInRace = 2,
            CheckpointInLap = 2,
            IsEndRace = false,
            IsEndLap = false,
            CurrentRaceCheckpoints = new List<int> { 1, 2, 3 },
            CurrentLapCheckpoints = new List<int> { 1, 2, 3 },
            BlockId = "bl",
            Speed = 100,
            Time = 15000
        });
    }
}
