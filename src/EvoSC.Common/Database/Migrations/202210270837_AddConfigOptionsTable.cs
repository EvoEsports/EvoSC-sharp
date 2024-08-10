using EvoSC.Common.Database.Models.Config;
using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Tags("Production")]
[Migration(1666859869)]
public class AddConfigOptionsTable : Migration
{
    public override void Up()
    {
        Create.Table(DbConfigOption.TableName)
            .WithColumn("Key").AsString().PrimaryKey()
            .WithColumn("Value").AsString();
    }

    public override void Down()
    {
        Delete.Table(DbConfigOption.TableName);
    }
}
