using FluentMigrator;

namespace EvoSC.Modules.Builtin.Player.Migrations;

[Migration(1665390396)]
public class AddPlayersTable : Migration
{
    public override void Up()
    {
        Create.Table("Players")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity();
    }

    public override void Down()
    {
        Delete.Table("Players");
    }
}
