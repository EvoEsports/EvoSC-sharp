using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Util;
using FluentMigrator;
using FluentMigrator.Postgres;

namespace EvoSC.Common.Database.Migrations;

[Tags("Production")]
[Migration(1665390396)]
public class AddPlayersTable : Migration
{
    public override void Up()
    {
        Create.Table(DbPlayer.TableName)
            .WithColumn("Id").AsInt32().PrimaryKey().Identity(PostgresGenerationType.ByDefault)
            .WithColumn("AccountId").AsString().Unique()
            .WithColumn("UbisoftName").AsString().Indexed()
            .WithColumn("NickName").AsString()
            .WithColumn("Zone").AsString().Nullable()
            .WithColumn("LastVisit").AsDateTime().Nullable()
            .WithColumn("CreatedAt").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("UpdatedAt").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);

        Insert.IntoTable("Players").Row(new
        {
            Id = PlayerUtils.NadeoPlayer.Id,
            AccountId = PlayerUtils.NadeoPlayer.AccountId,
            NickName = PlayerUtils.NadeoPlayer.NickName,
            UbisoftName = PlayerUtils.NadeoPlayer.UbisoftName,
            Zone = PlayerUtils.NadeoPlayer.Zone
        });

        // Postgres identity sequences aren't advanced by explicit inserts, so the next
        // auto-assigned player Id would collide with the seeded Nadeo row (Id = 1).
        IfDatabase("Postgres").Execute.Sql(
            "SELECT setval(pg_get_serial_sequence('\"Players\"', 'Id'), (SELECT MAX(\"Id\") FROM \"Players\"));");
    }

    public override void Down()
    {
        Delete.Table("Players");
    }
}
