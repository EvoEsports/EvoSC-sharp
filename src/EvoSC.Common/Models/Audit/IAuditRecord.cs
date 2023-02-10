using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Models.Audit;

public interface IAuditRecord
{
    public long Id { get; set; }
    public AuditEventStatus Status { get; set; }
    public string EventName { get; set; }
    public string Comment { get; set; }
    public string? Properties { get; set; }
    public IPlayer? Actor { get; set; }
    public DateTime CreatedAt { get; set; }
}

