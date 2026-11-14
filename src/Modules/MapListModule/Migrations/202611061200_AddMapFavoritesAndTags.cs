using EvoSC.Modules.Official.MapListModule.Database.Models;
using FluentMigrator;

namespace EvoSC.Modules.Official.MapListModule.Migrations;

[Tags("Production")]
[Migration(202611061200)]
public class AddMapFavoritesAndTags : Migration
{
    public override void Up()
    {
        Create.Table(DbMapFavorite.TableName)
            .WithColumn(nameof(DbMapFavorite.Id)).AsInt64().PrimaryKey().Identity()
            .WithColumn(nameof(DbMapFavorite.MapId)).AsInt64().Indexed()
            .WithColumn(nameof(DbMapFavorite.PlayerId)).AsInt64().Indexed();

        Create.Table(DbMapTag.TableName)
            .WithColumn(nameof(DbMapTag.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(DbMapTag.Name)).AsString(64).Unique();

        Create.Table(DbMapTagMap.TableName)
            .WithColumn(nameof(DbMapTagMap.MapTagId)).AsInt32().Indexed()
            .WithColumn(nameof(DbMapTagMap.MapId)).AsInt64().Indexed();
    }

    public override void Down()
    {
        Delete.Table(DbMapTagMap.TableName);
        Delete.Table(DbMapTag.TableName);
        Delete.Table(DbMapFavorite.TableName);
    }
}
