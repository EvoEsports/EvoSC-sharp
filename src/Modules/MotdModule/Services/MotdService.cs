using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MotdModule.Interfaces;

namespace EvoSC.Modules.Official.MotdModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class MotdService : IMotdService
{
    const string MotdUrl = "https://directus.evoesports.de/items/motd";
    
    private readonly IManialinkManager _manialink;
    private readonly IPlayerManagerService _playerManager;
    private readonly IHttpService _httpService;

    public MotdService(IManialinkManager manialink, IPlayerManagerService playerManager, IHttpService httpService)
    {
        _playerManager = playerManager;
        _manialink = manialink;
        _httpService = httpService;
    }
    
    public async Task ShowAsync()
    {
        var players = await _playerManager.GetOnlinePlayersAsync();
        //var accountId = PlayerUtils.ConvertLoginToAccountId(args.PlayerInfo.Login);
        //var player = await _playerManager.GetPlayerAsync(accountId);
        var player = players.FirstOrDefault();
        var motdText = await GetMotd();
        if(player is not null)
            await _manialink.SendManialinkAsync(player, "MotdModule.MotdTemplate", new { test="hallo"});
    }

    private async Task<string> GetMotd() => await _httpService.GetAsync(MotdUrl);
}
