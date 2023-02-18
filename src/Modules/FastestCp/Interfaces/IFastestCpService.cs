using EvoSC.Common.Remote.EventArgsModels;

namespace EvoSC.Modules.Official.FastestCp.Interfaces;

public interface IFastestCpService
{
    /// <summary>
    ///     Register the driven checkpoint time for a player if it's the fastest time the player has driven.
    /// </summary>
    /// <param name="args">IInformation about the waypoint</param>
    /// <returns></returns>
    public void RegisterCpTime(WayPointEventArgs args);

    /// <summary>
    ///     Get the best current checkpoint times
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public IEnumerable<IEnumerable<Task<FastestCpTime>>> GetCurrentBestCpTimes(string accountId, int limit);

    /// <summary>
    ///     Clear current fastest checkpoints
    /// </summary>
    /// <returns></returns>
    public void ResetCpTimes();
}
