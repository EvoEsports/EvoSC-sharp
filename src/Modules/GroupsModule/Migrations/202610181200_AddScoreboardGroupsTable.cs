using EvoSC.Modules.Official.GroupsModule.Database.Models;
using FluentMigrator;

namespace EvoSC.Modules.Official.GroupsModule.Migrations;

[Tags("Production")]
[Migration(202610181200)]
public class AddScoreboardGroupsTable : Migration
{
    public override void Up()
    {
        Create.Table(DbScoreboardGroup.TableName)
            .WithColumn(nameof(DbScoreboardGroup.GroupId)).AsInt32().PrimaryKey();
    }

    public override void Down()
    {
        Delete.Table(DbScoreboardGroup.TableName);
    }
}
