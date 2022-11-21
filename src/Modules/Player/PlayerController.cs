using System.Data.Common;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Database.Models;
using EvoSC.Common.Events;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Exceptions;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.Player.Interfaces;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.Player;

[Controller]
public class PlayerController : EvoScController<EventControllerContext>
{
    private readonly IPlayerService _playerService;
    
    public PlayerController(IPlayerService playerService) => _playerService = playerService;

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public Task OnPlayerConnect(object sender, PlayerConnectEventArgs args) =>
        _playerService.UpdateAndGreetPlayer(args.Login);
}
