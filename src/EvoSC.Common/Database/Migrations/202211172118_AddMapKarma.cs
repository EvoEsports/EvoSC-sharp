using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Migration(1668716322)]
public class AddMapKarma : Migration {
    public override void Up()
    {
        Create.Table("MapKarma")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Rating").AsInt16()
            .WithColumn("New").AsBoolean()
            .WithColumn("MapId").AsInt32()
            .WithColumn("PlayerId").AsInt32()
            .WithColumn("CreatedAt").AsDateTime()
            .WithColumn("UpdatedAt").AsDateTime();

        Create.ForeignKey("FK_MapKarma_Maps")
            .FromTable("MapKarma").ForeignColumn("MapId")
            .ToTable("Maps").PrimaryColumn("Id");

        Create.ForeignKey("FK_MapKarma_Players")
            .FromTable("MapKarma").ForeignColumn("PlayerId")
            .ToTable("Players").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.Table("MapKarma");
    }
}
