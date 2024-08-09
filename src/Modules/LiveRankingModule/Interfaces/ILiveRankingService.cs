using EvoSC.Common.Models.Callbacks;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.LiveRankingModule.Models;

namespace EvoSC.Modules.Official.LiveRankingModule.Interfaces;

public interface ILiveRankingService
{
    /// <summary>
    /// Determines if current mode is points based and requests scores.
    /// </summary>
    /// <returns></returns>
    public Task DetectModeAndRequestScoreAsync();

    /// <summary>
    /// Requests scores from the game mode.
    /// </summary>
    /// <returns></returns>
    public Task RequestScoresAsync();

    /// <summary>
    /// Maps the scores and displays the widget.
    /// </summary>
    /// <param name="scores"></param>
    /// <returns></returns>
    public Task MapScoresAndSendWidgetAsync(ScoresEventArgs scores);

    /// <summary>
    /// Maps the given ScoresEventArgs to LiveRankingPositions.
    /// </summary>
    /// <param name="scores"></param>
    /// <returns></returns>
    public Task<IEnumerable<LiveRankingPosition>> MapScoresAsync(ScoresEventArgs scores);

    /// <summary>
    /// Hides the live ranking widget for everyone.
    /// </summary>
    /// <returns></returns>
    public Task HideWidgetAsync();

    /// <summary>
    /// Determines whether a score should be displayed in the widget.
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public Task<bool> ScoreShouldBeDisplayedAsync(PlayerScore score);

    /// <summary>
    /// Returns whether the current mode is points based.
    /// </summary>
    /// <returns></returns>
    public Task<bool> CurrentModeIsPointsBasedAsync();

    /// <summary>
    /// Converts a PlayerScore to a LiveRankingPosition object.
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public Task<LiveRankingPosition> PlayerScoreToLiveRankingPositionAsync(PlayerScore score);
}
