using EvoSC.Common.Events.CoreEvents;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Models;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Models.StateMessages;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class SyncService(
    INatsJetstreamService natsJsService,
    IPlayerManagerService players,
    IServerClient server,
    ILogger<ISyncService> logger)
    : ISyncService
{
    public async Task PublishChatMessageAsync(PlayerChatGbxEventArgs args)
    {
        logger.LogTrace("Publishing chat message");
        var player = await players.GetOnlinePlayerAsync(PlayerUtils.ConvertLoginToAccountId(args.Login));
        var serverName = await server.Remote.GetServerNameAsync();
        var stateMessage = new ChatStateMessage
        {
            ClientId = serverName,
            Timestamp = DateTime.Now,
            Message = args.Text,
            AccountId = player.AccountId,
            NickName = player.NickName
        };

        await natsJsService.PublishMessageAsync(nameof(StateSubjects.ChatMessages), stateMessage);
    }

    public async Task PublishChatMessageAsync(ChatMessageEventArgs args)
    {
        logger.LogTrace("Publishing chat message");
        var serverName = await server.Remote.GetServerNameAsync();
        var stateMessage = new ChatStateMessage
        {
            ClientId = serverName,
            Timestamp = DateTime.Now,
            Message = args.MessageText,
            AccountId = args.Player.AccountId,
            NickName = args.Player.NickName
        };

        await natsJsService.PublishMessageAsync(nameof(StateSubjects.ChatMessages), stateMessage);
    }

    public async Task PublishMapFinishedAsync()
    {
        logger.LogTrace("Publishing map finished");
        var serverName = await server.Remote.GetServerNameAsync();
        await natsJsService.PublishMessageAsync(StateSubjects.MapFinished.GetIdentifier(),
            new StateMessage { ClientId = serverName, Timestamp = DateTime.Now });
    }

    public async Task PublishEndRoundAsync()
    {
        logger.LogTrace("Publishing end round");
        var serverName = await server.Remote.GetServerNameAsync();
        await natsJsService.PublishMessageAsync(StateSubjects.EndRound.GetIdentifier(),
            new StateMessage { ClientId = serverName, Timestamp = DateTime.Now });
    }

    public async Task PublishEndMatchAsync()
    {
        logger.LogTrace("Publishing end match");
        var serverName = await server.Remote.GetServerNameAsync();
        await natsJsService.PublishMessageAsync(StateSubjects.EndMatch.GetIdentifier(),
            new StateMessage { ClientId = serverName, Timestamp = DateTime.Now });
    }

    public async Task PublishWaypointAsync(WayPointEventArgs waypointEventArgs)
    {
        logger.LogTrace("Publishing waypoint");
        var player = await players.GetOnlinePlayerAsync(PlayerUtils.ConvertLoginToAccountId(waypointEventArgs.Login));
        var serverName = await server.Remote.GetServerNameAsync();
        var stateMessage = new WaypointMessage
        {
            ClientId = serverName,
            Timestamp = DateTime.Now,
            NickName = player.NickName,
            AccountId = player.AccountId,
            RaceTime = waypointEventArgs.RaceTime,
            CheckpointInRace = waypointEventArgs.CheckpointInRace,
            CurrentRaceCheckpoints = waypointEventArgs.CurrentRaceCheckpoints,
            IsEndRace = waypointEventArgs.IsEndRace,
            Speed = waypointEventArgs.Speed
        };

        await natsJsService.PublishMessageAsync(StateSubjects.Waypoint.GetIdentifier(), stateMessage);
    }

    public async Task PublishScoresAsync(ScoresEventArgs scoresEventArgs)
    {
        logger.LogTrace("Publishing scores");
        var serverName = await server.Remote.GetServerNameAsync();
        var message = new ScoresMessage
        {
            ClientId = serverName,
            Timestamp = DateTime.Now,
            Scores = scoresEventArgs.Players,
            TeamScores = scoresEventArgs.Teams,
            WinnerTeam = scoresEventArgs.WinnerTeam,
            WinnerPlayer = scoresEventArgs.WinnerPlayer,
            Section = scoresEventArgs.Section,
            UseTeams = scoresEventArgs.UseTeams
        };

        await natsJsService.PublishMessageAsync(StateSubjects.Scores.GetIdentifier(), message);
    }
}
