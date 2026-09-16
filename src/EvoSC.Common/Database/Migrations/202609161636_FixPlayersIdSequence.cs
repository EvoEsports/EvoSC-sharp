using EvoSC.Common.Database.Models.Player;
using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Tags("Production")]
[Migration(1789576614)]
public class FixPlayersIdSequence : Migration
{
    public override void Up()
    {
        // Postgres identity sequences aren't advanced by explicit inserts, so the seeded
        // Nadeo row (Id = 1) from AddPlayersTable left the sequence out of sync with the
        // next auto-assigned player Id. Fix it here so it also applies to databases that
        // already ran that migration before the sequence handling was added there.
        IfDatabase("Postgres").Execute.Sql(
            $"SELECT setval(pg_get_serial_sequence('\"{DbPlayer.TableName}\"', 'Id'), (SELECT MAX(\"Id\") FROM \"{DbPlayer.TableName}\"));");
    }

    public override void Down()
    {
    }
}
