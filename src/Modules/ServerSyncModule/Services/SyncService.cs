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
public class SyncService : ISyncService
{
    private readonly INatsConnectionService _nats;

    public SyncService(INatsConnectionService nats)
    {
        _nats = nats;
    }

    public Task PublishChatMessageAsync(IPlayer player, string message)
    {
        var stateMessage = new ChatStateStateMessage
        {
            Message = message,
            AccountId = player.AccountId,
            Timestamp = DateTime.Now,
            NickName = player.NickName
        };

        return _nats.PublishStateAsync(StateSubjects.ChatMessages, stateMessage);
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

        return _nats.PublishStateAsync(StateSubjects.PlayerState, stateMessage);
    }

    public Task PublishMapFinishedAsync()
    {
        return _nats.PublishStateAsync(StateSubjects.MapFinished, new StateMessage());
    }

    public Task PublishEndRoundAsync()
    {
        return _nats.PublishStateAsync(StateSubjects.EndRound, new StateMessage());
    }

    public Task PublishEndMatchAsync()
    {
        return _nats.PublishStateAsync(StateSubjects.EndMatch, new StateMessage());
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

        return _nats.PublishStateAsync(StateSubjects.Waypoint, stateMessage);
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

        return _nats.PublishStateAsync(StateSubjects.Scores, message);
    }
}
