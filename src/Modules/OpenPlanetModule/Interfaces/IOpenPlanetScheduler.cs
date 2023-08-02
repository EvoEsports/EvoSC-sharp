using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.OpenPlanetModule.Interfaces;

public interface IOpenPlanetScheduler
{
    /// <summary>
    /// Schedule a player to be kicked from the server.
    /// </summary>
    /// <param name="player">Player to kick.</param>
    /// <param name="renew">
    /// Whether to reset the timer (re-scheduling). If false and the player is already scheduled, the player will
    /// not be re-scheduled.
    /// </param>
    public void ScheduleKickPlayer(IPlayer player, bool renew);
    
    /// <summary>
    /// Schedule a player to be kicked from the server. Does not re-schedule if already scheduled.
    /// </summary>
    /// <param name="player">Player to kick.</param>
    public void ScheduleKickPlayer(IPlayer player) => ScheduleKickPlayer(player, false);
    
    /// <summary>
    /// Un-schedule a player from being kicked.
    /// </summary>
    /// <param name="player">The player to un-schedule.</param>
    /// <returns></returns>
    public bool UnScheduleKickPlayer(IPlayer player);
    
    /// <summary>
    /// Trigger events for any player that is due to be kicked.
    /// </summary>
    /// <returns></returns>
    public Task TriggerDuePlayerKicks();
    
    /// <summary>
    /// Check if a player is scheduled to be kicked.
    /// </summary>
    /// <param name="player">The player to check.</param>
    /// <returns></returns>
    public bool PlayerIsScheduledForKick(IPlayer player);
}
