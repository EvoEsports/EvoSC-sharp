using EvoSC.Common.Database.Models.Maps;
using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Tags("Production")]
[Migration(1710232879)]
public class AddMapDetailsTable : Migration
{
    public override void Up()
    {
        Create.Table(DbMapDetails.TableName)
            .WithColumn(nameof(DbMapDetails.MapId)).AsInt32().PrimaryKey()
            .WithColumn(nameof(DbMapDetails.AuthorTime)).AsInt32()
            .WithColumn(nameof(DbMapDetails.GoldTime)).AsInt32()
            .WithColumn(nameof(DbMapDetails.SilverTime)).AsInt32()
            .WithColumn(nameof(DbMapDetails.BronzeTime)).AsInt32()
            .WithColumn(nameof(DbMapDetails.Environment)).AsString()
            .WithColumn(nameof(DbMapDetails.Mood)).AsString()
            .WithColumn(nameof(DbMapDetails.Cost)).AsInt32()
            .WithColumn(nameof(DbMapDetails.MultiLap)).AsBoolean()
            .WithColumn(nameof(DbMapDetails.LapCount)).AsInt32()
            .WithColumn(nameof(DbMapDetails.MapStyle)).AsString()
            .WithColumn(nameof(DbMapDetails.MapType)).AsString()
            .WithColumn(nameof(DbMapDetails.CheckpointCount)).AsInt32();
    }

    public override void Down()
    {
        Delete.Table(nameof(DbMapDetails.TableName));
    }
}
