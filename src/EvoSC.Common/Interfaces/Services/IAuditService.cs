using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Audit;
using EvoSC.Common.Util.Auditing;
using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Interfaces.Services;

public interface IAuditService
{
    /// <summary>
    /// Log a new audit.
    /// </summary>
    /// <param name="eventName">The name of the audit.</param>
    /// <param name="status">Status of the audit.</param>
    /// <param name="actor">The user which caused the event.</param>
    /// <param name="comment">A comment or description of the event.</param>
    /// <param name="properties">Any properties or values associated with the event.</param>
    /// <returns></returns>
    internal Task LogAsync(string eventName, AuditEventStatus status, IPlayer actor, string comment,
        dynamic? properties);
    
    /// <summary>
    /// Begin auditing a new event.
    /// </summary>
    /// <param name="eventName">The name of the audit.</param>
    /// <returns></returns>
    public AuditEventBuilder NewEvent(string eventName);
    
    /// <summary>
    /// Begin auditing a new event.
    /// </summary>
    /// <param name="eventName">The name of the audit.</param>
    /// <returns></returns>
    public AuditEventBuilder NewEvent(Enum eventName);

    /// <summary>
    /// Begin auditing a new successful event.
    /// </summary>
    /// <param name="eventName">The name of the audit.</param>
    /// <returns></returns>
    public AuditEventBuilder NewSuccessEvent(string eventName) =>
        NewEvent(eventName).WithStatus(AuditEventStatus.Success);

    /// <summary>
    /// Begin auditing a new successful event.
    /// </summary>
    /// <param name="eventName">The name of the audit.</param>
    /// <returns></returns>
    public AuditEventBuilder NewSuccessEvent(Enum eventName) =>
        NewEvent(eventName.GetIdentifier(true)).WithStatus(AuditEventStatus.Success);
    
    /// <summary>
    /// Begin auditing a new error event.
    /// </summary>
    /// <param name="eventName">The name of the audit.</param>
    /// <returns></returns>
    public AuditEventBuilder NewErrorEvent(string eventName) =>
        NewEvent(eventName).WithStatus(AuditEventStatus.Error);

    /// <summary>
    /// Begin auditing a new error event.
    /// </summary>
    /// <param name="eventName">The name of the audit.</param>
    /// <returns></returns>
    public AuditEventBuilder NewErrorEvent(Enum eventName) =>
        NewEvent(eventName.GetIdentifier(true)).WithStatus(AuditEventStatus.Error);
    
    /// <summary>
    /// Begin auditing a new informational event.
    /// </summary>
    /// <param name="eventName">The name of the audit.</param>
    /// <returns></returns>
    public AuditEventBuilder NewInfoEvent(string eventName) =>
        NewEvent(eventName).WithStatus(AuditEventStatus.Info);

    /// <summary>
    /// Begin auditing a new informational event.
    /// </summary>
    /// <param name="eventName">The name of the audit.</param>
    /// <returns></returns>
    public AuditEventBuilder NewInfoEvent(Enum eventName) =>
        NewEvent(eventName.GetIdentifier(true)).WithStatus(AuditEventStatus.Info);
}
