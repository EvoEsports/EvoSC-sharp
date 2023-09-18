using EvoSC.Common.Remote.EventArgsModels;

namespace EvoSC.Modules.Official.MatchRankingModule.Interfaces;

public interface IMatchRankingService
{
    /// <summary>
    /// Consumes the scores event args, updates the match ranking and sends the updated manialink to all players.
    /// </summary>
    /// <returns></returns>
    Task UpdateAndShowScores(ScoresEventArgs scores);
    /// <summary>
    /// Send the manialink to all players.
    /// </summary>
    /// <returns></returns>
    Task SendManialink();
    /// <summary>
    /// Hide the manialink for all players.
    /// </summary>
    /// <returns></returns>
    Task HideManialink();
    /// <summary>
    /// Resets the match data to show an empty manialink on the next send.
    /// </summary>
    /// <returns></returns>
    Task ResetMatchData();
}
