using EvoSC.Common.Remote.EventArgsModels;

namespace EvoSC.Modules.Official.FastestCp.Interfaces;

public interface IFastestCpService
{
    /// <summary>
    ///     Register the driven checkpoint time for a player if it's the fastest time the player has driven.
    /// </summary>
    /// <param name="args">Information about the waypoint</param>
    /// <returns></returns>
    public Task RegisterCpTimeAsync(WayPointEventArgs args);

    /// <summary>
    ///     Clear current fastest checkpoints
    /// </summary>
    /// <returns></returns>
    public Task ResetCpTimesAsync();
}
