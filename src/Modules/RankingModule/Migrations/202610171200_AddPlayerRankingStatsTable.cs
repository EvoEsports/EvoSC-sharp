using EvoSC.Modules.Official.RankingModule.Database.Models;
using FluentMigrator;

namespace EvoSC.Modules.Official.RankingModule.Migrations;

[Tags("Production")]
[Migration(202610171200)]
public class AddPlayerRankingStatsTable : Migration
{
    public override void Up()
    {
        Create.Table(DbPlayerRankingStats.TableName)
            .WithColumn(nameof(DbPlayerRankingStats.Id)).AsInt64().PrimaryKey().Identity()
            .WithColumn(nameof(DbPlayerRankingStats.PlayerId)).AsInt64().NotNullable().Unique()
            .WithColumn(nameof(DbPlayerRankingStats.PlaytimeSeconds)).AsInt64().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(DbPlayerRankingStats.MapsPlayed)).AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(DbPlayerRankingStats.MapsWon)).AsInt32().NotNullable().WithDefaultValue(0);
    }

    public override void Down()
    {
        Delete.Table(DbPlayerRankingStats.TableName);
    }
}
