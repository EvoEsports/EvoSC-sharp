using EvoSC.Common.Database.Repository;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Evo.GeardownModule.Interfaces.Repositories;
using EvoSC.Modules.Evo.GeardownModule.Models.Database;
using LinqToDB;

namespace EvoSC.Modules.Evo.GeardownModule.Repositories;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class TourneyTimelineRepository : DbRepository, ITourneyTimelineRepository
{
    public TourneyTimelineRepository(IDbConnectionFactory dbConnFactory) : base(dbConnFactory)
    {
    }

    public Task AddTimeline(int matchId, Guid timelineId) =>
        Database.InsertAsync(new DbTourneyTimeline { MatchId = matchId, TimelineId = timelineId });
}
