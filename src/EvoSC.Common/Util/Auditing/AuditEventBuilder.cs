using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Util.Auditing;
using EvoSC.Common.Models.Audit;
using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Util.Auditing;

public class AuditEventBuilder : IAuditEventBuilder
{
    private readonly IAuditService _auditService;
 
    public string? EventName { get; private set; }
    public bool IsCanceled { get; private set; }
    public IPlayer? Actor { get; private set; }
    public AuditEventStatus Status { get; private set; }
    public dynamic? Properties { get; private set; }
    public string EventComment { get; private set; }
    public bool Activated { get; private set; }
    
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
        Activated = true;
    }

    public IAuditEventBuilder WithEventName(string eventName)
    {
        EventName = eventName;
        UnCancel();
        Activated = true;
        return this;
    }

    public IAuditEventBuilder WithEventName(Enum eventName) => WithEventName(eventName.GetIdentifier(true));

    public IAuditEventBuilder HavingProperties(dynamic properties)
    {
        Properties = properties;
        return this;
    }
    
    public IAuditEventBuilder WithStatus(AuditEventStatus status)
    {
        Status = status;
        return this;
    }

    public IAuditEventBuilder Success() => WithStatus(AuditEventStatus.Success);
    
    public IAuditEventBuilder Info() => WithStatus(AuditEventStatus.Info);
    
    public IAuditEventBuilder Error() => WithStatus(AuditEventStatus.Error);

    public IAuditEventBuilder CausedBy(IPlayer actor)
    {
        Actor = actor;
        return this;
    }

    public IAuditEventBuilder Cancel(bool isCancelled)
    {
        IsCanceled = isCancelled;
        return this;
    }
    
    public IAuditEventBuilder Cancel() => Cancel(true);
    
    public IAuditEventBuilder UnCancel() => Cancel(false);
    
    public IAuditEventBuilder Comment(string commentString)
    {
        EventComment = commentString;
        return this;
    }

    public async Task LogAsync(string comment)
    {
        if (IsCanceled)
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

    public Task LogAsync() => LogAsync(string.IsNullOrEmpty(EventComment) ? "" : EventComment);
}
