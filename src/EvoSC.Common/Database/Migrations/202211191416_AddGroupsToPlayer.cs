using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Migration(1668863905)]
public class AddGroupsToPlayer : Migration {
    public override void Up()
    {
        Alter.Table("Players")
            .AddColumn("Groups").AsString().Nullable();
    }

    public override void Down()
    {
        throw new NotImplementedException();
    }
}
