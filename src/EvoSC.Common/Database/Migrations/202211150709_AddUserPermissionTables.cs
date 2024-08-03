using EvoSC.Common.Database.Models.Permissions;
using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Tags("Production")]
[Migration(1668492589)]
public class AddUserPermissionTables : Migration
{
    public override void Up()
    {
        Create.Table(DbGroup.TableName)
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Title").AsString()
            .WithColumn("Description").AsString()
            .WithColumn("Icon").AsString().Nullable()
            .WithColumn("Color").AsString().Nullable()
            .WithColumn("Unrestricted").AsBoolean();

        Create.Table(DbPermission.TableName)
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString().Unique()
            .WithColumn("Description").AsString();

        Create.Table(DbGroupPermission.TableName)
            .WithColumn("GroupId").AsInt32().Indexed()
            .WithColumn("PermissionId").AsInt32().Indexed();

        Create.Table(DbUserGroup.TableName)
            .WithColumn("UserId").AsInt64().Indexed()
            .WithColumn("GroupId").AsInt32().Indexed()
            .WithColumn("Display").AsBoolean();
    }

    public override void Down()
    {
        Delete.Table(DbGroup.TableName);
        Delete.Table(DbPermission.TableName);
        Delete.Table(DbGroupPermission.TableName);
        Delete.Table(DbUserGroup.TableName);
    }
}
