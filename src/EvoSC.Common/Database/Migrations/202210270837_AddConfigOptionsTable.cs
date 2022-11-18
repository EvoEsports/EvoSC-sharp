using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Migration(1666859869)]
public class AddConfigOptionsTable : Migration
{
    public override void Up()
    {
        Create.Table("ConfigOptions")
            .WithColumn("Key").AsString().PrimaryKey()
            .WithColumn("Value").AsString();
    }

    public override void Down()
    {
        Delete.Table("ConfigOptions");
    }
}
