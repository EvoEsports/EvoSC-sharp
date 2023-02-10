using EvoSC.Common.Database.Models.AuditLog;
using EvoSC.Common.Models.Audit;

namespace EvoSC.Common.Interfaces.Database.Repository;

public interface IAuditRepository
{
    public Task<IAuditRecord> AddRecordAsync(DbAuditRecord record);
}
