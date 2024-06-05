using EvoSC.Modules.Official.LocalRecordsModule.Database.Models;
using FluentMigrator;

namespace EvoSC.Modules.Official.LocalRecordsModule;

[Tags("Production")]
[Migration(1717574116)]
public class AddLocalRecordsTable : Migration
{
    public override void Up()
    {
        Create.Table(DbLocalRecord.TableName)
            .WithColumn(nameof(DbLocalRecord.Id)).AsInt64().PrimaryKey().Identity()
            .WithColumn(nameof(DbLocalRecord.MapId)).AsInt64().Indexed()
            .WithColumn(nameof(DbLocalRecord.RecordId)).AsInt64().Indexed()
            .WithColumn(nameof(DbLocalRecord.Position)).AsInt32();
    }

    public override void Down()
    {
        Delete.Table(DbLocalRecord.TableName);
    }
}
