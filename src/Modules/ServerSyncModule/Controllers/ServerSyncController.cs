using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Util;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Events;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Events.Args;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Controllers;

[Controller]
public class ServerSyncController : EvoScController<IEventControllerContext>
{
    private readonly IServerClient _server;
    private readonly ISyncService _sync;
    private readonly IPlayerManagerService _players;

    public ServerSyncController(IServerClient server, ISyncService sync, IPlayerManagerService players)
    {
        _server = server;
        _sync = sync;
        _players = players;
    }

    [Subscribe(ServerSyncEvents.ChatMessage)]
    public async Task OnRemoteChatMessageAsync(object sender, ChatStateMessageEventArgs args)
    {
        var player = await _players.GetOnlinePlayerAsync(args.ChatMessage.AccountId);
        var chatMessage = FormattingUtils.FormatPlayerChatMessage(player, args.ChatMessage.Message, false);
        await _server.Chat.SendChatMessageAsync(chatMessage);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnLocalChatMessageAsync(object sender, PlayerChatGbxEventArgs args)
    {
        var player = await _players.GetOnlinePlayerAsync(PlayerUtils.ConvertLoginToAccountId(args.Login));
        await _sync.PublishChatMessageAsync(player, args.Text);
    }

    [Subscribe(GbxRemoteEvent.EndMap)]
    public Task OnEndMapAsync(object sender, MapGbxEventArgs args) =>
        _sync.PublishMapFinishedAsync();

    [Subscribe(ModeScriptEvent.EndRoundStart)]
    public Task OnEndRoundAsync(object sender, RoundEventArgs args) =>
        _sync.PublishEndRoundAsync();

    [Subscribe(GbxRemoteEvent.EndMatch)]
    public Task OnEndMatchAsync(object sender, EndMatchGbxEventArgs args) =>
        _sync.PublishEndMatchAsync();

    [Subscribe(ModeScriptEvent.WayPoint)]
    public async Task OnWayPointAsync(object sender, WayPointEventArgs args)
    {
        var player = await _players.GetOnlinePlayerAsync(PlayerUtils.ConvertLoginToAccountId(args.Login));
        await _sync.PublishWayPointAsync(player, args.RaceTime, args.CheckpointInRace, args.CurrentRaceCheckpoints,
            args.IsEndRace, args.Speed);
    }

    [Subscribe(ModeScriptEvent.Scores)]
    public async Task OnScoresAsync(object sender, ScoresEventArgs args)
    {
        await _sync.PublishScoresAsync(args.Players, args.Teams, args.WinnerTeam, args.WinnerPlayer, args.Section, args.UseTeams);
    }
}
