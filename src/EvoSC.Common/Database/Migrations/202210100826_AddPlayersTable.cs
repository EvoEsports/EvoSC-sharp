using EvoSC.Common.Util;
using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Tags("Production")]
[Migration(1665390396)]
public class AddPlayersTable : Migration
{
    public override void Up()
    {
        Create.Table("Players")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
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
    }

    public override void Down()
    {
        Delete.Table("Players");
    }
}
