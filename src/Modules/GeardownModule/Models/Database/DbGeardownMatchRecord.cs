using LinqToDB.Mapping;

namespace EvoSC.Modules.Evo.GeardownModule.Models.Database;

[Table(DbTourneyTimeline.TableName)]
public class DbTourneyTimeline
{
    public const string TableName = "TourneyTimelines";
    
    [Column]
    public long MatchId { get; set; }
    
    [Column]
    public Guid TimelineId { get; set; }
}
