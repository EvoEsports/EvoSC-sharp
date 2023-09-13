using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Migration(1694073143)]
public class UpdateConfigOptionsTable : Migration
{
    public override void Up()
    {
        Alter.Column("Value").OnTable(AddConfigOptionsTable.ConfigOptions)
            .AsCustom("TEXT");
    }

    public override void Down()
    {
    }
}
