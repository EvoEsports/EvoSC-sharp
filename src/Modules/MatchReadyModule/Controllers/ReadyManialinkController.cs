using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;

namespace EvoSC.Modules.Official.MatchReadyModule.Controllers;

[Controller]
public class ReadyManialinkController : ManialinkController
{
    private readonly IPlayerReadyService _playerReady;

    public ReadyManialinkController(IPlayerReadyService playerReady) => _playerReady = playerReady;

    public Task ReadyButtonAsync(bool isReady) => _playerReady.SetPlayerReadyStatusAsync(Context.Player, isReady);
}
