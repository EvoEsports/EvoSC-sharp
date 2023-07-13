using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.MotdModule.Controllers;

[Controller]
public class MotdManialinkController : ManialinkController
{
    private readonly IManialinkManager _manialink;
    
    public MotdManialinkController(IManialinkManager manialink)
    {
        _manialink = manialink;
    }
    
    public async Task ShowMotdAsync(IPlayer player)
    {
        await ShowAsync((IOnlinePlayer)player, "MotdModule.MotdTemplate", new { Name = player.NickName });
    }
}
