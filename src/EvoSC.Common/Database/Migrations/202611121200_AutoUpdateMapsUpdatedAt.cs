using EvoSC.Common.Database.Models.Maps;
using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Tags("Production")]
[Migration(1794484800)]
public class AutoUpdateMapsUpdatedAt : Migration
{
    private const string TriggerName = "Maps_UpdatedAt";

    public override void Up()
    {
        IfDatabase("MySql5").Execute.Sql(
            $"ALTER TABLE `{DbMap.TableName}` MODIFY `UpdatedAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;");

        IfDatabase("Postgres").Execute.Sql($"""
            CREATE OR REPLACE FUNCTION "{TriggerName}_fn"() RETURNS trigger AS $$
            BEGIN
                NEW."UpdatedAt" = CURRENT_TIMESTAMP;
                RETURN NEW;
            END;
            $$ LANGUAGE plpgsql;
            CREATE TRIGGER "{TriggerName}" BEFORE UPDATE ON "{DbMap.TableName}"
            FOR EACH ROW EXECUTE FUNCTION "{TriggerName}_fn"();
            """);

        IfDatabase("Sqlite").Execute.Sql($"""
            CREATE TRIGGER "{TriggerName}" AFTER UPDATE ON "{DbMap.TableName}"
            FOR EACH ROW
            BEGIN
                UPDATE "{DbMap.TableName}" SET "UpdatedAt" = CURRENT_TIMESTAMP WHERE "Id" = NEW."Id";
            END;
            """);
    }

    public override void Down()
    {
        IfDatabase("MySql5").Execute.Sql(
            $"ALTER TABLE `{DbMap.TableName}` MODIFY `UpdatedAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP;");

        IfDatabase("Postgres").Execute.Sql($"""
            DROP TRIGGER IF EXISTS "{TriggerName}" ON "{DbMap.TableName}";
            DROP FUNCTION IF EXISTS "{TriggerName}_fn"();
            """);

        IfDatabase("Sqlite").Execute.Sql($"DROP TRIGGER IF EXISTS \"{TriggerName}\";");
    }
}
