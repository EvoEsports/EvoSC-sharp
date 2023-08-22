using EvoSC.Modules.Official.MatchTrackerModule.Interfaces;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Stores;
using EvoSC.Modules.Official.MatchTrackerModule.Models;
using EvoSC.Modules.Official.MatchTrackerModule.Stores;
using Moq;

namespace EvoSC.Modules.Official.MatchTrackerModule.Tests.Stores;

public class DatabaseMatchTrackerStoreTests
{
    private Mock<IMatchRecordRepository> _repo = new();
    private IDatabaseMatchTrackerStore _store;

    public DatabaseMatchTrackerStoreTests()
    {
        _store = new DatabaseMatchMatchTrackerStore(_repo.Object);
    }
    
    [Fact]
    public async Task All_States_Are_Saved_From_Timeline()
    {
        var timeline = new MatchTimeline
        {
            States = new List<IMatchState>()
            {
                new MatchState {TimelineId = default, Status = MatchStatus.Unknown, Timestamp = default},
                new MatchState {TimelineId = default, Status = MatchStatus.Unknown, Timestamp = default},
                new MatchState {TimelineId = default, Status = MatchStatus.Unknown, Timestamp = default},
                new MatchState {TimelineId = default, Status = MatchStatus.Unknown, Timestamp = default},
                new MatchState {TimelineId = default, Status = MatchStatus.Unknown, Timestamp = default}
            },
        };

        await _store.SaveTimelineAsync(timeline);
        
        _repo.Verify(m => m.InsertStateAsync(It.IsAny<IMatchState>()), Times.Exactly(5));
    }

    [Fact]
    public async Task State_Is_Saved_To_Db()
    {
        var state = new MatchState {TimelineId = default, Status = MatchStatus.Unknown, Timestamp = default};
        await _store.SaveStateAsync(state);
        
        _repo.Verify(m => m.InsertStateAsync(state), Times.Once);
    }
}
