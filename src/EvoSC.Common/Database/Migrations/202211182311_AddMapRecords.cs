using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Migration(1668809535)]
public class AddMapRecords : Migration {
    public override void Up()
    {
        Create.Table("MapRecords")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("PlayerId").AsInt32()
            .WithColumn("MapId").AsInt32()
            .WithColumn("Score").AsInt32()
            .WithColumn("Rank").AsInt32()
            .WithColumn("Checkpoints").AsString()
            .WithColumn("CreatedAt").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("UpdatedAt").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);

        Create.ForeignKey("FK_MapRecords_Players")
            .FromTable("MapRecords").ForeignColumn("PlayerId")
            .ToTable("Players").PrimaryColumn("Id");
        
        Create.ForeignKey("FK_MapRecords_Maps")
            .FromTable("MapRecords").ForeignColumn("MapId")
            .ToTable("Maps").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.Table("MapRecords");
    }
}
