using EvoSC.Common.Database.Models.AuditLog;
using EvoSC.Common.Models.Audit;

namespace EvoSC.Common.Interfaces.Database.Repository;

public interface IAuditRepository
{
    /// <summary>
    /// Add a new audit record to the database.
    /// </summary>
    /// <param name="record">The audit to add.</param>
    /// <returns></returns>
    public Task<IAuditRecord> AddRecordAsync(DbAuditRecord record);
}
