using EvoSC.Common.Events.CoreEvents;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models;
using EvoSC.Common.Models.Callbacks;
using EvoSC.Common.Remote.EventArgsModels;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;

public interface ISyncService
{
    /// <summary>
    /// Publish a chat message to all connected servers.
    /// </summary>
    /// <param name="chatArgs">Chat arguments from a chat event.</param>
    /// <returns></returns>
    public Task PublishChatMessageAsync(ChatMessageEventArgs chatArgs);

    /// <summary>
    /// Publish a chat message to all connected servers.
    /// </summary>
    /// <param name="chatGbxArgs">Chat arguments from a chat event.</param>
    /// <returns></returns>
    public Task PublishChatMessageAsync(PlayerChatGbxEventArgs chatGbxArgs);

    /// <summary>
    /// Publish a map finished/ended event to all servers.
    /// </summary>
    /// <returns></returns>
    public Task PublishMapFinishedAsync();

    /// <summary>
    /// Publish end round event to all servers.
    /// </summary>
    /// <returns></returns>
    public Task PublishEndRoundAsync();

    /// <summary>
    /// Publish end match event to all servers.
    /// </summary>
    /// <returns></returns>
    public Task PublishEndMatchAsync();

    /// <summary>
    /// Publish a waypoint event to all servers.
    /// </summary>
    /// <param name="waypointEventArgs">Arguments sent by a waypoint event.</param>c
    /// <returns></returns>
    public Task PublishWaypointAsync(WayPointEventArgs waypointEventArgs);

    /// <summary>
    /// Publish scores of players and teams to all servers.
    /// </summary>
    /// <param name="scoresEventArgs">Arguments sent by a scores event.</param>
    public Task PublishScoresAsync(ScoresEventArgs scoresEventArgs);
}
