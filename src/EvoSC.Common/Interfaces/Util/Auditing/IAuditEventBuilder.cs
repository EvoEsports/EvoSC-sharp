using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Audit;

namespace EvoSC.Common.Interfaces.Util.Auditing;

public interface IAuditEventBuilder
{
    /// <summary>
    /// The name of the event.
    /// </summary>
    public string? EventName { get; }

    /// <summary>
    /// Whether this event has been canceled.
    /// </summary>
    public bool IsCanceled { get; }

    /// <summary>
    /// The user that caused the event.
    /// </summary>
    public IPlayer? Actor { get; }

    /// <summary>
    /// The status of the audit.
    /// </summary>
    public AuditEventStatus Status { get; }

    /// <summary>
    /// Properties associated with the event.
    /// </summary>
    public dynamic? Properties { get; }

    /// <summary>
    /// Comment or description of the audit.
    /// </summary>
    public string EventComment { get; }

    /// <summary>
    /// Whether this audit is activated; if event name is set.
    /// Overrides IsCanceled.
    /// </summary>
    public bool Activated { get; }

    /// <summary>
    /// Set the name of the event.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <returns></returns>
    public IAuditEventBuilder WithEventName(string eventName);

    /// <summary>
    /// Set the name of the event.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <returns></returns>
    public IAuditEventBuilder WithEventName(Enum eventName);

    /// <summary>
    /// Set the properties of this event.
    /// </summary>
    /// <param name="properties">The properties to set.</param>
    /// <returns></returns>
    public IAuditEventBuilder HavingProperties(dynamic properties);

    /// <summary>
    /// Set the status of this audit.
    /// </summary>
    /// <param name="status">The status to set this audit event to.</param>
    /// <returns></returns>
    public IAuditEventBuilder WithStatus(AuditEventStatus status);

    /// <summary>
    /// Set the status of this audit as successful.
    /// </summary>
    /// <returns></returns>
    public IAuditEventBuilder Success();

    /// <summary>
    /// Set the status of this audit as informational.
    /// </summary>
    /// <returns></returns>
    public IAuditEventBuilder Info();

    /// <summary>
    /// Set the status of this audit as error.
    /// </summary>
    /// <returns></returns>
    public IAuditEventBuilder Error();

    /// <summary>
    /// Set the user that caused the event.
    /// </summary>
    /// <param name="actor">The user that caused the event.</param>
    /// <returns></returns>
    public IAuditEventBuilder CausedBy(IPlayer actor);

    /// <summary>
    /// Set whether this event is canceled or not.
    /// </summary>
    /// <param name="isCancelled">If true, the audit will not be logged.</param>
    /// <returns></returns>
    public IAuditEventBuilder Cancel(bool isCancelled);

    /// <summary>
    /// Cancel this audit and prevent it from being logged.
    /// </summary>
    /// <returns></returns>
    public IAuditEventBuilder Cancel();

    /// <summary>
    /// Un-cancel and allow logging of this audit.
    /// </summary>
    /// <returns></returns>
    public IAuditEventBuilder UnCancel();

    /// <summary>
    /// Set the comment/description of this event.
    /// </summary>
    /// <param name="commentString">The comment for this audit.</param>
    /// <returns></returns>
    public IAuditEventBuilder Comment(string commentString);

    /// <summary>
    /// Log the audit now.
    /// </summary>
    /// <param name="comment">The comment/description of this event.</param>
    /// <exception cref="InvalidOperationException">Thrown when the event name is not set.</exception>
    public Task LogAsync(string comment);

    /// <summary>
    /// Log the audit now.
    /// </summary>
    /// <returns></returns>
    public Task LogAsync();
}
