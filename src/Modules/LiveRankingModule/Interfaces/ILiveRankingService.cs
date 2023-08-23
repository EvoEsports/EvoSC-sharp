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

    Task OnPlayerWaypointAsync(WayPointEventArgs args);

    Task OnPlayerGiveupAsync(PlayerUpdateEventArgs args);

    Task OnBeginMapAsync(MapEventArgs args);

    Task OnEndMapAsync(MapEventArgs args);

    Task OnStartRoundAsync(RoundEventArgs args);

    Task OnPodiumStartAsync(PodiumEventArgs args);

    Task OnEndRoundAsync(RoundEventArgs args);

    Task OnBeginMatchAsync();

    Task OnEndMatchAsync(EndMatchGbxEventArgs args);

}
