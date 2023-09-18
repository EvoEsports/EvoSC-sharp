namespace EvoSC.Modules.Evo.GeardownModule.Interfaces.Repositories;

public interface ITourneyTimelineRepository
{
    /// <summary>
    /// Add a match ID to a timeline.
    /// </summary>
    /// <param name="matchId">ID of the match.</param>
    /// <param name="timelineId">Tracking ID of the timeline.</param>
    /// <returns></returns>
    public Task AddTimelineAsync(int matchId, Guid timelineId);
}
