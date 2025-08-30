using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;

namespace EvoSC.Modules.Official.MatchReadyModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class ReadyManialinkService(IReadyService readyService, IManialinkManager manialinks, IPlayerManagerService playerManager) : IReadyManialinkService
{
    public async Task SendWidgetAsync()
    {
        var requiredPlayers = readyService.Players.Count();
        var playersReady = readyService.ReadyPlayers.Count();
        
        var trans = manialinks.CreateTransaction();
        var allPlayers = await playerManager.GetOnlinePlayersAsync();
        
        foreach (var player in allPlayers)
        {
            var isReady = readyService.ReadyPlayers.Contains(player);
            var showButton = readyService.Players.Contains(player);

            await trans.SendManialinkAsync(player, "MatchReadyModule.ReadyWidget", new
            {
                isReady, requiredPlayers, playersReady, showButton
            });
        }

        await trans.CommitAsync();
    }

    public async Task SendWidgetAsync(IPlayer player)
    {
        var requiredPlayers = readyService.Players.Count();
        var playersReady = readyService.ReadyPlayers.Count();
        
        var isReady = readyService.ReadyPlayers.Contains(player);
        var showButton = readyService.Players.Contains(player);

        await manialinks.SendManialinkAsync(player, "MatchReadyModule.ReadyWidget", new
        {
            isReady, requiredPlayers, playersReady, showButton
        });
    }
}
