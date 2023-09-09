using EvoSC.Common.Remote.EventArgsModels;

namespace EvoSC.Modules.Official.MatchRankingModule.Interfaces;

public interface IMatchRankingService
{
    Task OnScores(ScoresEventArgs scores);
    Task OnPodiumStartAsync(PodiumEventArgs scores);
    Task SendManialink();
    Task SendManialink(string playerLogin);
    Task HideManialink();
    Task Reset();
}
