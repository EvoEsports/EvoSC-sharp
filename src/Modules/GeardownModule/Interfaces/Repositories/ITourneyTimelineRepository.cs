namespace EvoSC.Modules.Evo.GeardownModule.Interfaces.Repositories;

public interface ITourneyTimelineRepository
{
    public Task AddTimeline(int matchId, Guid timelineId);
}
