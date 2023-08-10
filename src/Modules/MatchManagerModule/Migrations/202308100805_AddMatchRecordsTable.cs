using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Modules.Official.MatchManagerModule.Models.Database;
using FluentMigrator;

namespace EvoSC.Modules.Official.MatchManagerModule.Migrations;

[Migration(1691647540)]
public class AddMatchRecordsTable : Migration
{
    public const string TableName = "MatchRecords";

    public override void Up()
    {
        Create.Table(TableName)
            .WithColumn(nameof(DbMatchRecord.Id)).AsInt64().PrimaryKey().Identity()
            .WithColumn(nameof(DbMatchRecord.TimelineId)).AsGuid().Indexed()
            .WithColumn(nameof(DbMatchRecord.Status)).AsString()
            .WithColumn(nameof(DbMatchRecord.Report)).AsCustom("TEXT")
            .WithColumn(nameof(DbMatchRecord.Timestamp)).AsDateTime();
    }

    public override void Down()
    {
        Delete.Table(TableName);
    }
}
