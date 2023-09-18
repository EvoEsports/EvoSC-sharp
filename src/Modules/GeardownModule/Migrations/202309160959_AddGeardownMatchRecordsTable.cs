using EvoSC.Modules.Evo.GeardownModule.Models.Database;
using FluentMigrator;

namespace EvoSC.Modules.Evo.GeardownModule.Migrations;

[Migration(1694847552)]
public class AddGeardownMatchRecordsTable : Migration
{
    public override void Up()
    {
        Create.Table(DbTourneyTimeline.TableName)
            .WithColumn(nameof(DbTourneyTimeline.MatchId)).AsInt64().Indexed()
            .WithColumn(nameof(DbTourneyTimeline.TimelineId)).AsGuid().Indexed();
    }

    public override void Down()
    {
        Delete.Table(DbTourneyTimeline.TableName);
    }
}
