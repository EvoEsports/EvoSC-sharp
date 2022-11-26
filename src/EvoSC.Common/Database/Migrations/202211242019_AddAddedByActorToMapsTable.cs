using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Migration(1669317693)]
public class AddAddedByActorToMapsTable : Migration
{
    public override void Up()
    {
        Alter.Table("Maps")
            .AddColumn("AddedBy").AsInt32()
            .WithColumnDescription("The player who added the map");
    }

    public override void Down()
    {
        Delete.Column("AddedBy").FromTable("Maps");
    }
}
