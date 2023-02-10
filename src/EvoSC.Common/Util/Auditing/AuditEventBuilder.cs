using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Audit;
using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Util.Auditing;

public class AuditEventBuilder
{
    private readonly IAuditService _auditService;
    
    public string? EventName { get; private set; }
    public bool Canceled { get; private set; }
    public IPlayer? Actor { get; private set; }
    public AuditEventStatus Status { get; private set; }
    public dynamic? Properties { get; private set; }
    public string Comment { get; private set; }
    
    internal AuditEventBuilder(IAuditService auditService)
    {
        _auditService = auditService;
        Status = AuditEventStatus.Info;
        Cancel();
    }
    
    internal AuditEventBuilder(IAuditService auditService, string eventName)
    {
        _auditService = auditService;
        EventName = eventName;
        Status = AuditEventStatus.Info;
    }

    public AuditEventBuilder WithEventName(string eventName)
    {
        EventName = eventName;
        UnCancel();
        return this;
    }

    public AuditEventBuilder WithEventName(Enum eventName) => WithEventName(eventName.GetIdentifier(true));

    public AuditEventBuilder WithProperties(dynamic properties)
    {
        Properties = properties;
        return this;
    }
    
    public AuditEventBuilder WithStatus(AuditEventStatus status)
    {
        Status = status;
        return this;
    }

    public AuditEventBuilder AsSuccess() => WithStatus(AuditEventStatus.Success);
    public AuditEventBuilder AsInfo() => WithStatus(AuditEventStatus.Info);
    public AuditEventBuilder AsError() => WithStatus(AuditEventStatus.Error);

    public AuditEventBuilder CausedBy(IPlayer actor)
    {
        Actor = actor;
        return this;
    }

    public AuditEventBuilder Cancel(bool isCancelled)
    {
        Canceled = isCancelled;
        return this;
    }
    
    public AuditEventBuilder Cancel() => Cancel(true);
    public AuditEventBuilder UnCancel() => Cancel(false);


    public AuditEventBuilder WithComment(string comment)
    {
        Comment = comment;
        return this;
    }

    public async Task Log(string comment)
    {
        if (Canceled)
        {
            return;
        }
        
        if (EventName == null)
        {
            throw new InvalidOperationException("Audit event name not specified.");
        }

        await _auditService.LogAsync(EventName, Status, Actor, comment, Properties);
        
        // prevent further executions
        Cancel();
    }

    public Task Log() => Log(string.IsNullOrEmpty(Comment) ? "" : Comment);
}
