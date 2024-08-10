using EvoSC.Common.Database.Models.AuditLog;
using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Tags("Production")]
[Migration(1689575375)]
public class UpdateAuditLogTableDataTypes : Migration
{
    public override void Up()
    {
        Alter.Column("Properties").OnTable(DbAuditRecord.TableName)
            .AsCustom("TEXT");
        
        Alter.Column("Comment").OnTable(DbAuditRecord.TableName)
            .AsCustom("TEXT");
    }

    public override void Down()
    {
        
    }
}
