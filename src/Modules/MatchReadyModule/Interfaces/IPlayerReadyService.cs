using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchReadyModule.Interfaces;

public interface IPlayerReadyService
{
    public Task SetPlayerReadyStatusAsync(IPlayer player, bool isReady);
    public Task ResetReadyWidgetAsync();
}
