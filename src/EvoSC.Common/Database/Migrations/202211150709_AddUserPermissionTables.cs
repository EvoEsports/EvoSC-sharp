using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Tags("XPEvo", "Production")]
[Migration(1668492589)]
public class AddUserPermissionTables : Migration
{
    public override void Up()
    {
        Create.Table("Groups")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Title").AsString()
            .WithColumn("Description").AsString()
            .WithColumn("Icon").AsString().Nullable()
            .WithColumn("Color").AsString().Nullable()
            .WithColumn("Unrestricted").AsBoolean();

        Create.Table("Permissions")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString().Unique()
            .WithColumn("Description").AsString();

        Create.Table("GroupPermissions")
            .WithColumn("GroupId").AsInt32().Indexed()
            .WithColumn("PermissionId").AsInt32().Indexed();

        Create.Table("UserGroups")
            .WithColumn("UserId").AsInt64().Indexed()
            .WithColumn("GroupId").AsInt32().Indexed()
            .WithColumn("Display").AsBoolean();
    }

    public override void Down()
    {
        Delete.Table("Groups");
        Delete.Table("Permissions");
        Delete.Table("GroupPermissions");
        Delete.Table("UserGroups");
    }
}
