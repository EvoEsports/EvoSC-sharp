using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Migration(1668714621)]
public class AddMapStatisticsTable : Migration {
    public override void Up()
    {
        Create.Table("MapStatistics")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("MapId").AsInt32().Unique()
            .WithColumn("TimesPlayed").AsInt32()
            .WithColumn("Cooldown").AsInt32()
            .WithColumn("LastPlayed").AsDateTime()
            .WithColumn("AmountSkipped").AsInt32()
            .WithColumn("CreatedAt").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("UpdatedAt").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);

        Create.ForeignKey("FK_MapStatistics_Maps")
            .FromTable("MapStatistics").ForeignColumn("MapId")
            .ToTable("Maps").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.Table("MapStatistics");
    }
}
