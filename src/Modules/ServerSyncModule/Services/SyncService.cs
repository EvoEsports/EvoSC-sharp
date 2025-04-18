using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models;
using EvoSC.Common.Models.Callbacks;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Models;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Models.StateMessages;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class SyncService(INatsJetstreamService natsJsService) : ISyncService
{
    public Task PublishChatMessageAsync(IPlayer player, string message)
    {
        var stateMessage = new ChatStateStateMessage
        {
            Message = message,
            AccountId = player.AccountId,
            Timestamp = DateTime.Now,
            NickName = player.NickName
        };

        return natsJsService.PublishMessageAsync(nameof (StateSubjects.ChatMessages), stateMessage);
    }

    public Task PublishPlayerStateAsync(IPlayer player, long position, IEnumerable<long> scores, IEnumerable<long> checkpointScores, IEnumerable<long> times)
    {
        var stateMessage = new PlayerStateUpdateMessage
        {
            Timestamp = DateTime.Now,
            AccountId = player.AccountId,
            NickName = player.NickName,
            Scores = scores,
            Position = position,
            CheckpointScores = checkpointScores,
            Times = times
        };

        return natsJsService.PublishMessageAsync<PlayerStateUpdateMessage>(nameof (StateSubjects.PlayerState), stateMessage);
    }

    public Task PublishMapFinishedAsync()
    {
        return natsJsService.PublishMessageAsync(nameof (StateSubjects.MapFinished), new StateMessage());
    }

    public Task PublishEndRoundAsync()
    {
        return natsJsService.PublishMessageAsync(nameof (StateSubjects.EndRound), new StateMessage());
    }

    public Task PublishEndMatchAsync()
    {
        return natsJsService.PublishMessageAsync(nameof (StateSubjects.EndMatch), new StateMessage());
    }

    public Task PublishWayPointAsync(IOnlinePlayer player, int raceTime, int checkpointInRace,
        IEnumerable<int> currentRaceCheckpoints, bool isEndRace, float speed)
    {
        var stateMessage = new WaypointMessage
        {
            NickName = player.NickName,
            AccountId = player.AccountId,
            RaceTime = raceTime,
            CheckpointInRace = checkpointInRace,
            CurrentRaceCheckpoints = currentRaceCheckpoints,
            IsEndRace = isEndRace,
            Speed = speed
        };

        return natsJsService.PublishMessageAsync(nameof (StateSubjects.Waypoint), stateMessage);
    }

    public Task PublishScoresAsync(IEnumerable<PlayerScore?> playerScores, IEnumerable<TeamScore?> teamScores, int winnerTeam,
        string? winnerPlayer, ModeScriptSection section, bool useTeams)
    {
        var message = new ScoresMessage
        {
            Scores = playerScores,
            TeamScores = teamScores,
            WinnerTeam = winnerTeam,
            WinnerPlayer = winnerPlayer,
            Section = section,
            UseTeams = useTeams
        };

        return natsJsService.PublishMessageAsync(nameof (StateSubjects.Scores), message);
    }
}
