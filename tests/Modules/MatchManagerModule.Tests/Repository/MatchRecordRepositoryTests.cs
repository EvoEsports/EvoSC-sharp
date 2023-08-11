using System.Text.Json;
using EvoSC.Common.Database.Migrations;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Modules.Official.MatchManagerModule.Migrations;
using EvoSC.Modules.Official.MatchManagerModule.Models;
using EvoSC.Modules.Official.MatchManagerModule.Models.Database;
using EvoSC.Modules.Official.MatchManagerModule.Repository;
using EvoSC.Testing.Database;
using LinqToDB;
using Microsoft.Extensions.Logging;
using Moq;

namespace MatchManagerModule.Tests.Repository;

public class MatchRecordRepositoryTests
{
    private static (MatchRecordRepository, IDbConnectionFactory) CreateNewRepository()
    {
        var factory = TestDbSetup.CreateDb(typeof(AddMatchRecordsTable).Assembly);
        var logger = new Mock<ILogger<MatchRecordRepository>>();
        return (new MatchRecordRepository(factory, logger.Object), factory);
    }

    [Fact]
    public async Task Match_State_Is_Inserted()
    {
        var (repo, dbFactory) = CreateNewRepository();

        var state = new MatchState
        {
            TimelineId = Guid.NewGuid(), 
            Status = MatchStatus.Running, 
            Timestamp = DateTime.Now
        };

        var json = JsonSerializer.Serialize(state);
        await repo.InsertStateAsync(state);

        var record = await dbFactory.GetConnection()
            .GetTable<DbMatchRecord>()
            .FirstOrDefaultAsync();

        Assert.NotNull(record);
        Assert.Equal(state.TimelineId, record.TimelineId);
        Assert.Equal(MatchStatus.Running, record.Status);
        Assert.Equal(state.Timestamp, record.Timestamp);
        Assert.Equal(json, record.Report);
    }
}
