using EvoSC.Common.Database.Models.AuditLog;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Models.Audit;
using LinqToDB;

namespace EvoSC.Common.Database.Repository.Audit;

public class AuditRepository : DbRepository, IAuditRepository
{
    public AuditRepository(IDbConnectionFactory dbConnFactory) : base(dbConnFactory)
    {
    }

    public async Task<IAuditRecord> AddRecordAsync(DbAuditRecord record)
    {
        var id = await Database.InsertWithIdentityAsync(record);
        record.Id = (long)id;
        return record;
    }
}
