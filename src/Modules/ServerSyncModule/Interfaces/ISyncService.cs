using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models;
using EvoSC.Common.Models.Callbacks;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;

public interface ISyncService
{
    /// <summary>
    /// Publish a chat message to all connected servers.
    /// </summary>
    /// <param name="player">The player that sent the chat message.</param>
    /// <param name="message">The message text.</param>
    /// <returns></returns>
    public Task PublishChatMessageAsync(IPlayer player, string message);

    /// <summary>
    /// Publish a player state to all connected servers.
    /// </summary>
    /// <param name="player">The player that was updated.</param>
    /// <param name="position">New position of the player.</param>
    /// <param name="scores">Scores of the player.</param>
    /// <param name="checkpointScores">Checkpoint scores/times of the player.</param>
    /// <param name="times">Times the player have driven.</param>
    /// <returns></returns>
    public Task PublishPlayerStateAsync(IPlayer player, long position, IEnumerable<long> scores, IEnumerable<long> checkpointScores, IEnumerable<long> times);

    /// <summary>
    /// Publish the scores of a player to all connected servers.
    /// </summary>
    /// <param name="player">The player to update.</param>
    /// <param name="scores">Scores of the player.</param>
    /// <returns></returns>
    public Task PublishPlayerStateAsync(IPlayer player, IEnumerable<long> scores) =>
        PublishPlayerStateAsync(player, 0, scores, Array.Empty<long>(), Array.Empty<long>());

    /// <summary>
    /// Publish the position of a player to all servers.
    /// </summary>
    /// <param name="player">Player to publish the position for.</param>
    /// <param name="position">New position of the player.</param>
    /// <returns></returns>
    public Task PublishPlayerStateAsync(IPlayer player, long position) =>
        PublishPlayerStateAsync(player, position, Array.Empty<long>(), Array.Empty<long>(), Array.Empty<long>());

    /// <summary>
    /// Publish the position and scores of a player to all servers.
    /// </summary>
    /// <param name="player">The player to update.</param>
    /// <param name="position">New position of the player.</param>
    /// <param name="scores">Scores of the player.</param>
    /// <returns></returns>
    public Task PublishPlayerStateAsync(IPlayer player, long position, IEnumerable<long> scores) =>
        PublishPlayerStateAsync(player, position, scores, Array.Empty<long>(), Array.Empty<long>());

    /// <summary>
    /// Publish the position, scores and checkpoints of a player to all servers.
    /// </summary>
    /// <param name="player">The player to update.</param>
    /// <param name="position">New position of the player.</param>
    /// <param name="scores">Scores of the player.</param>
    /// <param name="checkpointScores">Checkpoint scores/times of the player.</param>
    /// <returns></returns>
    public Task PublishPlayerStateAsync(IPlayer player, long position, IEnumerable<long> scores, IEnumerable<long> checkpointScores)
        => PublishPlayerStateAsync(player, position, scores, checkpointScores, Array.Empty<long>());

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
    /// <param name="player"></param>
    /// <param name="raceTime"></param>
    /// <param name="checkpointInRace"></param>
    /// <param name="currentRaceCheckpoints"></param>
    /// <param name="isEndRace"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    public Task PublishWayPointAsync(IOnlinePlayer player, int raceTime, int checkpointInRace,
        IEnumerable<int> currentRaceCheckpoints, bool isEndRace, float speed);

    /// <summary>
    /// Publish scores of players and teams to all servers.
    /// </summary>
    ///
    public Task PublishScoresAsync(IEnumerable<PlayerScore?> playerScores, IEnumerable<TeamScore?> teamScores, int winnerTeam, string? winnerPlayer, ModeScriptSection section, bool useTeams);
}
