using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;

namespace EvoSC.Modules.Official.MatchReadyModule.Controllers;

[Controller]
public class ReadyManialinkController(IReadyService readyService) : ManialinkController
{
    public Task ReadyButtonAsync(bool isReady) => readyService.SetPlayerReadyStatusAsync(Context.Player, isReady);
}
