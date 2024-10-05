using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.RoundRankingModule.Interfaces;
using EvoSC.Modules.Official.RoundRankingModule.Models;

namespace EvoSC.Modules.Official.RoundRankingModule.Controllers;

[Controller]
public class RoundRankingCommandsController(IRoundRankingService roundRankingService)
    : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("test", "fakeplayer")]
    public async Task TestRoundRankingCommandAsync()
    {
        var player1 = new OnlinePlayer
        {
            State = PlayerState.Playing, AccountId = "*fakeplayer1*", NickName = "Test Player #1"
        };
        var player2 = new OnlinePlayer
        {
            State = PlayerState.Playing, AccountId = "*fakeplayer2*", NickName = "TwoTwoTwoTwo"
        };
        var player3 = new OnlinePlayer
        {
            State = PlayerState.Playing, AccountId = "*fakeplayer3*", NickName = "Thrid Test Player"
        };
        
        Thread.Sleep(500);
        
        await roundRankingService.ConsumeCheckpointDataAsync(new CheckpointData
        {
            Player = player1,
            CheckpointId = 0,
            Time = RaceTime.FromMilliseconds(1200),
            IsFinish = false,
            IsDNF = false
        });
        await roundRankingService.ConsumeCheckpointDataAsync(new CheckpointData
        {
            Player = player2,
            CheckpointId = 0,
            Time = RaceTime.FromMilliseconds(1250),
            IsFinish = false,
            IsDNF = false
        });
        await roundRankingService.ConsumeCheckpointDataAsync(new CheckpointData
        {
            Player = player3,
            CheckpointId = 0,
            Time = RaceTime.FromMilliseconds(1000),
            IsFinish = false,
            IsDNF = false
        });
        
        Thread.Sleep(2500);

        await roundRankingService.ConsumeCheckpointDataAsync(new CheckpointData
        {
            Player = player2,
            CheckpointId = 1,
            Time = RaceTime.FromMilliseconds(2000),
            IsFinish = false,
            IsDNF = false
        });
        await roundRankingService.ConsumeCheckpointDataAsync(new CheckpointData
        {
            Player = player3,
            CheckpointId = 1,
            Time = RaceTime.FromMilliseconds(2100),
            IsFinish = false,
            IsDNF = false
        });
        
        Thread.Sleep(1000);
        
        await roundRankingService.ConsumeCheckpointDataAsync(new CheckpointData
        {
            Player = player1,
            CheckpointId = 1,
            Time = RaceTime.FromMilliseconds(2555),
            IsFinish = false,
            IsDNF = false
        });
        await roundRankingService.ConsumeCheckpointDataAsync(new CheckpointData
        {
            Player = player3,
            CheckpointId = 0,
            Time = RaceTime.FromMilliseconds(0),
            IsFinish = false,
            IsDNF = true 
        });
        
        Thread.Sleep(1000);

        await roundRankingService.ConsumeCheckpointDataAsync(new CheckpointData
        {
            Player = player2,
            CheckpointId = 2,
            Time = RaceTime.FromMilliseconds(3500),
            IsFinish = false,
            IsDNF = false
        });
        
        Thread.Sleep(1500);
        
        await roundRankingService.ConsumeCheckpointDataAsync(new CheckpointData
        {
            Player = player1,
            CheckpointId = 2,
            Time = RaceTime.FromMilliseconds(4555),
            IsFinish = false,
            IsDNF = false
        });
        
        Thread.Sleep(2000);
        
        await roundRankingService.ConsumeCheckpointDataAsync(new CheckpointData
        {
            Player = player1,
            CheckpointId = 3,
            Time = RaceTime.FromMilliseconds(7500),
            IsFinish = false,
            IsDNF = false
        });
        
        Thread.Sleep(2000);
        
        await roundRankingService.ConsumeCheckpointDataAsync(new CheckpointData
        {
            Player = player1,
            CheckpointId = 4,
            Time = RaceTime.FromMilliseconds(8000),
            IsFinish = true,
            IsDNF = false
        });
    }
}
