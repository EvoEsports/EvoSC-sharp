using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.OpenPlanetModule.Interfaces;

public interface IOpenPlanetScheduler
{
    public void ScheduleKickPlayer(IPlayer player, bool renew);
    public void ScheduleKickPlayer(IPlayer player) => ScheduleKickPlayer(player, false);
    public bool UnScheduleKickPlayer(IPlayer player);
    public Task TriggerDuePlayerKicks();
    public bool PlayerIsScheduledForKick(IPlayer player);
}
