using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Migration(1668341591)]
public class AddMapFavoritesTable : Migration
{
    public override void Up()
    {
        Create.Table("MapFavorites")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("PlayerId").AsInt32()
            .WithColumn("MapId").AsInt32()
            .WithColumn("CreatedAt").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("UpdatedAt").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);

        Create.ForeignKey("FK_MapFavorites_Players")
            .FromTable("MapFavorites").ForeignColumn("PlayerId")
            .ToTable("Players").PrimaryColumn("Id");

        Create.ForeignKey("FK_MapFavorites_Maps")
            .FromTable("MapFavorites").ForeignColumn("MapId")
            .ToTable("Maps").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.Table("MapFavorites");
    }
}
