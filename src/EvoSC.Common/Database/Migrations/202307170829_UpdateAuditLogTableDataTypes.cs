using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Migration(1689575375)]
public class UpdateAuditLogTableDataTypes : Migration
{
    public override void Up()
    {
        Alter.Column("Properties").OnTable(AddAuditLogTable.AuditLog)
            .AsCustom("TEXT");
        
        Alter.Column("Comment").OnTable(AddAuditLogTable.AuditLog)
            .AsCustom("TEXT");
    }

    public override void Down()
    {
        
    }
}
