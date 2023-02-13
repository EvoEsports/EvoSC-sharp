using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Audit;
using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Util.Auditing;

public class AuditEventBuilder
{
    private readonly IAuditService _auditService;
    
    /// <summary>
    /// The name of the event.
    /// </summary>
    public string? EventName { get; private set; }
    
    /// <summary>
    /// Whether this event has been canceled.
    /// </summary>
    public bool IsCanceled { get; private set; }
    
    /// <summary>
    /// The user that caused the event.
    /// </summary>
    public IPlayer? Actor { get; private set; }
    
    /// <summary>
    /// The status of the audit.
    /// </summary>
    public AuditEventStatus Status { get; private set; }
    
    /// <summary>
    /// Properties associated with the event.
    /// </summary>
    public dynamic? Properties { get; private set; }
    
    /// <summary>
    /// Comment or description of the audit.
    /// </summary>
    public string EventComment { get; private set; }
    
    /// <summary>
    /// Whether this audit is activated; if event name is set.
    /// Overrides IsCanceled.
    /// </summary>
    public bool Activated { get; private set; }
    
    /// <summary>
    /// Create a new audit builder from the audit service.
    /// </summary>
    /// <param name="auditService">The audit service to use.</param>
    internal AuditEventBuilder(IAuditService auditService)
    {
        _auditService = auditService;
        Status = AuditEventStatus.Info;
        Cancel();
    }
    
    /// <summary>
    /// Create a new builder with a specific event name
    /// </summary>
    /// <param name="auditService">The audit service to use.</param>
    /// <param name="eventName">The name of the event.</param>
    internal AuditEventBuilder(IAuditService auditService, string eventName)
    {
        _auditService = auditService;
        EventName = eventName;
        Status = AuditEventStatus.Info;
        Activated = true;
    }

    /// <summary>
    /// Set the name of the event.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <returns></returns>
    public AuditEventBuilder WithEventName(string eventName)
    {
        EventName = eventName;
        UnCancel();
        Activated = true;
        return this;
    }

    /// <summary>
    /// Set the name of the event.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <returns></returns>
    public AuditEventBuilder WithEventName(Enum eventName) => WithEventName(eventName.GetIdentifier(true));

    /// <summary>
    /// Set the properties of this event.
    /// </summary>
    /// <param name="properties">The properties to set.</param>
    /// <returns></returns>
    public AuditEventBuilder HavingProperties(dynamic properties)
    {
        Properties = properties;
        return this;
    }
    
    /// <summary>
    /// Set the status of this audit.
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public AuditEventBuilder WithStatus(AuditEventStatus status)
    {
        Status = status;
        return this;
    }

    /// <summary>
    /// Set the status of this audit as successful.
    /// </summary>
    /// <returns></returns>
    public AuditEventBuilder Success() => WithStatus(AuditEventStatus.Success);
    
    /// <summary>
    /// Set the status of this audit as informational.
    /// </summary>
    /// <returns></returns>
    public AuditEventBuilder Info() => WithStatus(AuditEventStatus.Info);
    
    /// <summary>
    /// Set the status of this audit as error.
    /// </summary>
    /// <returns></returns>
    public AuditEventBuilder Error() => WithStatus(AuditEventStatus.Error);

    /// <summary>
    /// Set the user that caused the event.
    /// </summary>
    /// <param name="actor">The user that caused the event.</param>
    /// <returns></returns>
    public AuditEventBuilder CausedBy(IPlayer actor)
    {
        Actor = actor;
        return this;
    }

    /// <summary>
    /// Set whether this event is canceled or not.
    /// </summary>
    /// <param name="isCancelled">If true, the audit will not be logged.</param>
    /// <returns></returns>
    public AuditEventBuilder Cancel(bool isCancelled)
    {
        IsCanceled = isCancelled;
        return this;
    }
    
    /// <summary>
    /// Cancel this audit and prevent it from being logged.
    /// </summary>
    /// <returns></returns>
    public AuditEventBuilder Cancel() => Cancel(true);
    
    /// <summary>
    /// Un-cancel and allow logging of this audit.
    /// </summary>
    /// <returns></returns>
    public AuditEventBuilder UnCancel() => Cancel(false);
    
    /// <summary>
    /// Set the comment/description of this event.
    /// </summary>
    /// <param name="comment"></param>
    /// <returns></returns>
    public AuditEventBuilder Comment(string commentString)
    {
        EventComment = commentString;
        return this;
    }

    /// <summary>
    /// Log the audit now.
    /// </summary>
    /// <param name="comment">The comment/description of this event.</param>
    /// <exception cref="InvalidOperationException"></exception>
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

    /// <summary>
    /// Log the audit now.
    /// </summary>
    /// <returns></returns>
    public Task LogAsync() => LogAsync(string.IsNullOrEmpty(EventComment) ? "" : EventComment);
}
