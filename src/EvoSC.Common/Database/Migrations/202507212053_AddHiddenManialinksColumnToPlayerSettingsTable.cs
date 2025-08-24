using EvoSC.Common.Database.Models.Config;
using EvoSC.Common.Database.Models.Player;
using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Tags("Production")]
[Migration(1753124078)]
public class AddHiddenManialinksColumnToPlayerSettingsTable : Migration
{
    public override void Up()
    {
        Alter.Table(DbPlayerSettings.TableName)
            .AddColumn("HiddenManialinks")
            .AsCustom("TEXT")
            .Nullable();
    }

    public override void Down()
    {
    }
}
