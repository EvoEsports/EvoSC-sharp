using EvoSC.Common.Database.Migrations;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Audit;
using LinqToDB.Mapping;

namespace EvoSC.Common.Database.Models.AuditLog;

[Table(AddAuditLogTable.AuditLog)]
public class DbAuditRecord : IAuditRecord
{
    [PrimaryKey, Identity]
    public long Id { get; set; }

    [Column]
    public AuditEventStatus Status { get; set; }
    
    IPlayer? IAuditRecord.Actor
    {
        get => Actor;
        set => Actor = new DbPlayer(value);
    }
    
    [Column]
    public string EventName { get; set; }
    
    [Column]
    public string Comment { get; set; }
    
    [Column]
    public string? Properties { get; set; }

    [Column]
    public DateTime CreatedAt { get; set; }

    [Column]
    public long ActorId { get; set; }
    
    [Association(ThisKey = nameof(ActorId), OtherKey = nameof(DbPlayer.Id))]
    public DbPlayer? Actor { get; set; }
}
