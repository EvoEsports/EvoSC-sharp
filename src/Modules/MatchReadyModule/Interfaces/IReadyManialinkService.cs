using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchReadyModule.Interfaces;

public interface IReadyManialinkService
{
    public Task SendWidgetAsync();
    public Task SendWidgetAsync(IPlayer player);
}
