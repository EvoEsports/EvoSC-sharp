using EvoSC.Common.Models.Callbacks;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.LiveRankingModule.Models;

namespace EvoSC.Modules.Official.LiveRankingModule.Interfaces;

public interface ILiveRankingService
{
    public Task InitializeAsync();

    public Task HandleScoresAsync(ScoresEventArgs scores);

    public Task<LiveRankingPosition> PlayerScoreToLiveRankingPositionAsync(PlayerScore score);

    public Task<bool> ScoreShouldBeDisplayedAsync(PlayerScore score, bool isPointsBased);

    public Task HideWidgetAsync();
}
