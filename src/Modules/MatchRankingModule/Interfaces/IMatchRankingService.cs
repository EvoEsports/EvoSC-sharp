using EvoSC.Common.Remote.EventArgsModels;

namespace EvoSC.Modules.Official.MatchRankingModule.Interfaces;

public interface IMatchRankingService
{
    Task UpdateAndShowScores(ScoresEventArgs scores);
    Task SendManialink();
    Task HideManialink();
    Task ResetMatchData();
}
