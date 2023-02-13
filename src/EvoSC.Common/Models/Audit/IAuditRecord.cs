using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Models.Audit;

public interface IAuditRecord
{
    /// <summary>
    /// The database ID of the audit.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// The status of the audit. Info, Success or Error.
    /// </summary>
    public AuditEventStatus Status { get; set; }
    
    /// <summary>
    /// The name of the event that occured.
    /// </summary>
    public string EventName { get; set; }
    
    /// <summary>
    /// Comment or description of the event.
    /// </summary>
    public string Comment { get; set; }
    
    /// <summary>
    /// Properties or values associated with the event.
    /// </summary>
    public string? Properties { get; set; }
    
    /// <summary>
    /// The user that caused the event.
    /// </summary>
    public IPlayer? Actor { get; set; }
    
    /// <summary>
    /// The time at which the event/audit occured.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

