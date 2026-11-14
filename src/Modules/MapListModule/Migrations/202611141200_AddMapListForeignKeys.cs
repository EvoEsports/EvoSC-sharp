using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Modules.Official.MapListModule.Database.Models;
using FluentMigrator;

namespace EvoSC.Modules.Official.MapListModule.Migrations;

[Tags("Production")]
[Migration(202611141200)]
public class AddMapListForeignKeys : Migration
{
    public override void Up()
    {
        // rows pointing to deleted maps, players or tags would make the constraints fail
        // (SQLite has no foreign keys here, so there is nothing to clean up for it)
        IfDatabase("Postgres").Execute.Sql(CleanupSql('"'));
        IfDatabase("MySql").Execute.Sql(CleanupSql('`'));

        // one favorite per player and map, and one link per tag and map
        Create.Index("UQ_MapFavorites_PlayerId_MapId")
            .OnTable(DbMapFavorite.TableName)
            .OnColumn(nameof(DbMapFavorite.PlayerId)).Ascending()
            .OnColumn(nameof(DbMapFavorite.MapId)).Ascending()
            .WithOptions().Unique();

        Create.Index("UQ_MapTagMap_MapTagId_MapId")
            .OnTable(DbMapTagMap.TableName)
            .OnColumn(nameof(DbMapTagMap.MapTagId)).Ascending()
            .OnColumn(nameof(DbMapTagMap.MapId)).Ascending()
            .WithOptions().Unique();

        Create.ForeignKey("FK_MapFavorites_Maps")
            .FromTable(DbMapFavorite.TableName).ForeignColumn(nameof(DbMapFavorite.MapId))
            .ToTable(DbMap.TableName).PrimaryColumn(nameof(DbMap.Id))
            .OnDelete(System.Data.Rule.Cascade);

        Create.ForeignKey("FK_MapFavorites_Players")
            .FromTable(DbMapFavorite.TableName).ForeignColumn(nameof(DbMapFavorite.PlayerId))
            .ToTable(DbPlayer.TableName).PrimaryColumn(nameof(DbPlayer.Id))
            .OnDelete(System.Data.Rule.Cascade);

        Create.ForeignKey("FK_MapTagMap_Maps")
            .FromTable(DbMapTagMap.TableName).ForeignColumn(nameof(DbMapTagMap.MapId))
            .ToTable(DbMap.TableName).PrimaryColumn(nameof(DbMap.Id))
            .OnDelete(System.Data.Rule.Cascade);

        Create.ForeignKey("FK_MapTagMap_MapTags")
            .FromTable(DbMapTagMap.TableName).ForeignColumn(nameof(DbMapTagMap.MapTagId))
            .ToTable(DbMapTag.TableName).PrimaryColumn(nameof(DbMapTag.Id))
            .OnDelete(System.Data.Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Index("UQ_MapTagMap_MapTagId_MapId").OnTable(DbMapTagMap.TableName);
        Delete.Index("UQ_MapFavorites_PlayerId_MapId").OnTable(DbMapFavorite.TableName);
        Delete.ForeignKey("FK_MapTagMap_MapTags").OnTable(DbMapTagMap.TableName);
        Delete.ForeignKey("FK_MapTagMap_Maps").OnTable(DbMapTagMap.TableName);
        Delete.ForeignKey("FK_MapFavorites_Players").OnTable(DbMapFavorite.TableName);
        Delete.ForeignKey("FK_MapFavorites_Maps").OnTable(DbMapFavorite.TableName);
    }

    private static string CleanupSql(char q) =>
        $"DELETE FROM {q}{DbMapFavorite.TableName}{q} WHERE {q}MapId{q} NOT IN (SELECT {q}Id{q} FROM {q}{DbMap.TableName}{q}) " +
        $"OR {q}PlayerId{q} NOT IN (SELECT {q}Id{q} FROM {q}{DbPlayer.TableName}{q});" +
        $"DELETE FROM {q}{DbMapTagMap.TableName}{q} WHERE {q}MapId{q} NOT IN (SELECT {q}Id{q} FROM {q}{DbMap.TableName}{q}) " +
        $"OR {q}MapTagId{q} NOT IN (SELECT {q}Id{q} FROM {q}{DbMapTag.TableName}{q});";
}
