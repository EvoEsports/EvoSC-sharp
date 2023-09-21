using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.SponsorsModule.Interfaces;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.SponsorsModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class SponsorsService : ISponsorsService
{
    private const string Template = "SponsorsModule.Sponsors";

    private readonly IPlayerManagerService _playerManager;
    private readonly IManialinkManager _manialinkManager;
    private readonly ILogger<SponsorsService> _logger;
    private readonly IPlayerCacheService _playerCacheService;
    private readonly IEvoScBaseConfig _config;

    public SponsorsService(IPlayerManagerService playerManager, IManialinkManager manialinkManager,
        ILogger<SponsorsService> logger, IPlayerCacheService playerCacheService, IEvoScBaseConfig config)
    {
        _playerManager = playerManager;
        _manialinkManager = manialinkManager;
        _logger = logger;
        _playerCacheService = playerCacheService;
        _config = config;
    }

    public async Task ShowWidgetToAllSpectators()
    {
        _logger.LogInformation("Showing widget to everyone.");
        await _manialinkManager.SendManialinkAsync(Template, GetData());
        
        /*
        foreach (var player in players)
        {
            //if (player.State == PlayerState.Spectating)
            //{
            _logger.LogInformation("Showing widget to {user}.", player.NickName);
            await ShowWidget(player.GetLogin());
            //}
        }
        */
    }

    private dynamic GetData()
    {
        return new
        {
            sponsorImageUrls = new List<string>
            {
                "https://cdn.evotm.com/tm2020/xpevo/Xperion.png",
                "https://cdn.evotm.com/tm2020/xpevo/Evo.png",
                "https://cdn.evotm.com/tm2020/xpevo/Lenovo.png",
            },
            headerColor = _config.Theme.UI.HeaderBackgroundColor,
            primaryColor = _config.Theme.UI.PrimaryColor,
            logoUrl = _config.Theme.UI.LogoWhiteUrl,
            playerRowBackgroundColor = _config.Theme.UI.PlayerRowBackgroundColor
        };
    }
    
    public Task HideWidgetFromEveryone()
    {
        return _manialinkManager.HideManialinkAsync(Template);
    }

    public Task ShowWidget(string playerLogin)
    {
        return _manialinkManager.SendManialinkAsync(playerLogin, Template, new { });
    }

    public async Task ShowOrHide(PlayerInfoChangedGbxEventArgs playerInfoChangedArgs)
    {
        var player = await _playerManager.GetOnlinePlayerAsync(playerInfoChangedArgs.PlayerInfo.Login);

        if (player.State == PlayerState.Spectating)
        {
            await ShowWidget(playerInfoChangedArgs.PlayerInfo.Login);
        }
        else
        {
            await _manialinkManager.HideManialinkAsync(player, Template);
        }
    }
}
