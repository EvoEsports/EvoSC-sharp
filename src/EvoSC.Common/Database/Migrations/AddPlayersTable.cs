using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Migration(1665390396)]
public class AddPlayersTable : Migration
{
    public override void Up()
    {
        Create.Table("Players")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Login").AsString().Unique()
            .WithColumn("UbisoftName").AsString().Indexed()
            .WithColumn("Zone").AsString().Nullable()
            .WithColumn("LastVisit").AsDateTime().Nullable()
            .WithColumn("CreatedAt").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("UpdatedAt").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);
    }

    public override void Down()
    {
        Delete.Table("Players");
    }
}
