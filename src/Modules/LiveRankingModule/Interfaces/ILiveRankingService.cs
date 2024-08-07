using EvoSC.Common.Remote.EventArgsModels;

namespace EvoSC.Modules.Official.LiveRankingModule.Interfaces;

public interface ILiveRankingService
{
    public Task Initialize();

    public Task HandleScoresAsync(ScoresEventArgs scores);

    public Task HideWidgetAsync();
}
