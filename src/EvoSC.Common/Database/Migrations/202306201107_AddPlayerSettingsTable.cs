using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Tags("Production")]
[Migration(1687252035)]
public class AddPlayerSettingsTable : Migration
{
    public const string PlayerSettings = "PlayerSettings";
    
    public override void Up()
    {
        Create.Table(PlayerSettings)
            .WithColumn("PlayerId").AsInt64().Unique()
            .WithColumn("DisplayLanguage").AsString().WithDefaultValue("en");
    }

    public override void Down()
    {
        Delete.Table(PlayerSettings);
    }
}
