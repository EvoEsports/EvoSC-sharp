using EvoSC.Common.Models.Callbacks;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.LiveRankingModule.Models;

namespace EvoSC.Modules.Official.LiveRankingModule.Interfaces;

public interface ILiveRankingService
{
    public Task DetectModeAndRequestScoreAsync();

    public Task RequestScoresAsync();

    public Task MapScoresAndSendWidgetAsync(ScoresEventArgs scores);

    public Task HideWidgetAsync();

    public Task<bool> ScoreShouldBeDisplayedAsync(PlayerScore score);

    public Task<bool> CurrentModeIsPointsBasedAsync();

    public Task<LiveRankingPosition> PlayerScoreToLiveRankingPositionAsync(PlayerScore score);
}
