using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchReadyModule.Interfaces;

public interface IPlayerReadyService
{
    public bool MatchIsStarted { get; } 
    
    public Task SetPlayerReadyStatusAsync(IPlayer player, bool isReady);
    public Task ResetReadyWidgetAsync(bool resetRequired);

    public Task SetMatchStartedAsync();

    public Task<bool> PlayerIsReadyAsync(IPlayer player);

    public Task SendWidgetAsync(IPlayer player);
    
    public Task UpdateWidgetAsync();
}
