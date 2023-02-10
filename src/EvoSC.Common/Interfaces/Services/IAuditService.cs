using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Models.Audit;
using EvoSC.Common.Models.Audit;
using EvoSC.Common.Util.Auditing;
using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Interfaces.Services;

public interface IAuditService
{
    internal Task LogAsync(string eventName, AuditEventStatus status, IPlayer actor, string comment,
        dynamic? properties);
    public AuditEventBuilder NewEvent(string eventName);
    public AuditEventBuilder NewEvent(Enum eventName);

    public AuditEventBuilder NewSuccessEvent(string eventName) =>
        NewEvent(eventName).WithStatus(AuditEventStatus.Success);

    public AuditEventBuilder NewSuccessEvent(Enum eventName) =>
        NewEvent(eventName.GetIdentifier(true)).WithStatus(AuditEventStatus.Success);
    
    public AuditEventBuilder NewErrorEvent(string eventName) =>
        NewEvent(eventName).WithStatus(AuditEventStatus.Error);

    public AuditEventBuilder NewErrorEvent(Enum eventName) =>
        NewEvent(eventName.GetIdentifier(true)).WithStatus(AuditEventStatus.Error);
}
