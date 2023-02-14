using System.Text.Json;
using EvoSC.Common.Database.Models.AuditLog;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Audit;
using EvoSC.Common.Util.Auditing;
using EvoSC.Common.Util.EnumIdentifier;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Services;

public class AuditService : IAuditService
{
    private readonly ILogger<AuditService> _logger;
    private readonly IAuditRepository _auditRepository;
    
    public AuditService(ILogger<AuditService> logger, IAuditRepository auditRepository)
    {
        _logger = logger;
        _auditRepository = auditRepository;
    }

    private async Task LogDatabaseAsync(DbAuditRecord auditRecord)
    {
        await _auditRepository.AddRecordAsync(auditRecord);
    }

    private void LogLogger(DbAuditRecord auditRecord)
    {
        var logLevel = auditRecord.Status switch
        {
            AuditEventStatus.Error => LogLevel.Error,
            _ => LogLevel.Information
        };

        _logger.Log(logLevel, "status={Status} event={Id} actor={Actor} comment={Comment} actionData={ActionData}",
            auditRecord.Status,
            auditRecord.EventName,
            auditRecord.Actor?.AccountId ?? "<unknown>",
            $"\"{auditRecord.Comment}\"",
            auditRecord.Properties
        );
    }

    async Task IAuditService.LogAsync(string eventName, AuditEventStatus status, IPlayer actor, string comment,
        dynamic? properties)
    {
        var serializedData = properties != null ? JsonSerializer.Serialize(properties) : null;

        var auditRecord = new DbAuditRecord
        {
            Status = status,
            Actor = new DbPlayer(actor),
            CreatedAt = DateTime.UtcNow,
            ActorId = actor.Id,
            EventName = eventName,
            Comment = comment,
            Properties = serializedData
        };

        await LogDatabaseAsync(auditRecord);
        LogLogger(auditRecord);
    }

    public AuditEventBuilder NewEvent(string eventName) => new(this, eventName);

    public AuditEventBuilder NewEvent(Enum eventName) => NewEvent(eventName.GetIdentifier(true));
}
