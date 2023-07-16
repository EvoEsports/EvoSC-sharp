using System.Diagnostics.CodeAnalysis;
using FluentMigrator;

namespace EvoSC.Modules.Official.MotdModule.Database.Migrations;

[ExcludeFromCodeCoverage(Justification = "Database is not testable.")]
[Migration(1689431400)]
public class MotdMigration : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Motd")
            .WithColumn("PlayerId").AsInt32().PrimaryKey()
            .WithColumn("Hidden").AsBoolean();
    }

    public override void Down()
    {
        Delete.Table("Motd");
    }
}
