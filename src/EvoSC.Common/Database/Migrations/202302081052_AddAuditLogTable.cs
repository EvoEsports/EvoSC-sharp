using FluentMigrator;

namespace EvoSC.Common.Database.Migrations;

[Migration(1675849925)]
public class AddAuditLogTable : Migration
{
    public const string AuditLog = "AuditLog";
    
    public override void Up()
    {
        Create.Table(AuditLog)
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Status").AsInt16()
            .WithColumn("EventName").AsString().Indexed()
            .WithColumn("Comment").AsString()
            .WithColumn("Properties").AsString().Nullable()
            .WithColumn("ActorId").AsInt64().Nullable()
            .WithColumn("CreatedAt").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);
    }

    public override void Down()
    {
        Delete.Table(AuditLog);
    }
}
