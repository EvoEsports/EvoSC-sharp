using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;

namespace EvoSC.Modules.Official.MatchReadyModule.Controllers;

[Controller]
public class ReadyManialinkController(IPlayerReadyService playerReady) : ManialinkController
{
    public Task ReadyButtonAsync(bool isReady) => playerReady.SetPlayerReadyStatusAsync(Context.Player, isReady);
}
