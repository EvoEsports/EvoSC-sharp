using EvoSC.Common.Database.Models.Config;
using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Tags("Production")]
[Migration(1694073143)]
public class UpdateConfigOptionsTable : Migration
{
    public override void Up()
    {
        Alter.Column("Value").OnTable(DbConfigOption.TableName)
            .AsCustom("TEXT");
    }

    public override void Down()
    {
    }
}
