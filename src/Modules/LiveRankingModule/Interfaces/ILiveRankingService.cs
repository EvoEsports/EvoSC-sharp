using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.LiveRankingModule.Models;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.LiveRankingModule.Interfaces;

public interface ILiveRankingService
{
    /// <summary>
    /// Called on when module is enabled
    /// </summary>
    /// <returns></returns>
    Task OnEnableAsync();

    /// <summary>
    /// Called on when module is disabled
    /// </summary>
    /// <returns></returns>
    Task OnDisableAsync();

    /// <summary>
    /// Called when a player passes a checkpoint.
    /// </summary>
    /// <returns></returns>
    Task OnPlayerWaypointAsync(WayPointEventArgs args);

    /// <summary>
    /// Called when a player retires from the current round.
    /// </summary>
    /// <returns></returns>
    Task OnPlayerGiveupAsync(PlayerUpdateEventArgs args);

    /// <summary>
    /// Called when a new map starts.
    /// </summary>
    /// <returns></returns>
    Task OnBeginMapAsync(MapEventArgs args);

    /// <summary>
    /// Called when a map ends.
    /// </summary>
    /// <returns></returns>
    Task OnEndMapAsync(MapEventArgs args);

    /// <summary>
    /// Called when a round ends.
    /// </summary>
    /// <returns></returns>
    Task OnEndRoundAsync(RoundEventArgs args);

    /// <summary>
    /// Called when a new round starts.
    /// </summary>
    /// <returns></returns>
    Task OnStartRoundAsync(RoundEventArgs args);

    /// <summary>
    /// Called when the podium sequence starts.
    /// </summary>
    /// <returns></returns>
    Task OnPodiumStartAsync(PodiumEventArgs args);

    /// <summary>
    /// Sends a manialink.
    /// </summary>
    /// <returns></returns>
    Task SendManialink();

    /// <summary>
    /// Called when a new match starts.
    /// </summary>
    /// <returns></returns>
    Task OnBeginMatchAsync();

    /// <summary>
    /// Called when a match ends.
    /// </summary>
    /// <returns></returns>
    Task OnEndMatchAsync(EndMatchGbxEventArgs args);
    
    /// <summary>
    /// Calculates and sets the diffs of given live ranking positions.
    /// </summary>
    /// <returns></returns>
    Task CalculateDiffs(List<ExpandedLiveRankingPosition> rankings);
    
    /// <summary>
    /// Resets the live ranking data.
    /// </summary>
    /// <returns></returns>
    Task ResetLiveRanking();
}
