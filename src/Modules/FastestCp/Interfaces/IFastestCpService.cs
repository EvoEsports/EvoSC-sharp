using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.FastestCp.Models;

namespace EvoSC.Modules.Official.FastestCp.Interfaces;

public interface IFastestCpService
{
    /// <summary>
    ///     Register the driven checkpoint time for a player if it's the fastest time the player has driven.
    /// </summary>
    /// <param name="args">Information about the waypoint</param>
    /// <returns></returns>
    public void RegisterCpTime(WayPointEventArgs args);

    /// <summary>
    ///     Get the best current checkpoint times
    /// </summary>
    /// <param name="limit">The amount of records to show</param>
    /// <returns></returns>
    public Task<PlayerCpTime[][]> GetCurrentBestCpTimes(int limit);

    /// <summary>
    ///     Clear current fastest checkpoints
    /// </summary>
    /// <returns></returns>
    public void ResetCpTimes();
}
