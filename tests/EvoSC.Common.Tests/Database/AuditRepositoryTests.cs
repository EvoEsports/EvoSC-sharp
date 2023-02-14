using System.Linq;
using System.Threading.Tasks;
using EvoSC.Common.Database.Models.AuditLog;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Database.Repository.Audit;
using EvoSC.Common.Database.Repository.Stores;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Models.Audit;
using EvoSC.Common.Tests.Database.Setup;
using LinqToDB;
using Xunit;

namespace EvoSC.Common.Tests.Database;

public class AuditRepositoryTests
{
    private static (AuditRepository, IDbConnectionFactory) CreateNewRepository()
    {
        var factory = TestDbSetup.CreateFullDb();
        return (new AuditRepository(factory), factory);
    }

    [Fact]
    public async Task Audit_Record_Added()
    {
        var (repo, dbFactory) = CreateNewRepository();
        var player = await TestHelper.AddTestPlayer(dbFactory);

        await repo.AddRecordAsync(new DbAuditRecord
        {
            Status = AuditEventStatus.Success,
            Actor = new DbPlayer(player),
            EventName = "MyEvent",
            Comment = "My Comment.",
            Properties = "{\"MyProp\": 1337}",
            ActorId = player.Id
        });

        var record = await dbFactory.GetConnection()
            .GetTable<DbAuditRecord>()
            .LoadWith(t => t.Actor)
            .FirstOrDefaultAsync();
        
        Assert.NotNull(record);
        Assert.Equal(AuditEventStatus.Success, record.Status);
        Assert.NotNull(record.Actor);
        Assert.Equal(player.Id, record.ActorId);
        Assert.Equal("MyEvent", record.EventName);
        Assert.Equal("My Comment.", record.Comment);
        Assert.Equal("{\"MyProp\": 1337}", record.Properties);
    }
}
