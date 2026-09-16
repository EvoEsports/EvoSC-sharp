using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.MatchReadyModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class ReadyManialinkService(IReadyService readyService, IManialinkManager manialinks, IPlayerManagerService playerManager, ILogger<ReadyManialinkService> logger) : IReadyManialinkService
{
    public async Task SendWidgetAsync()
    {
        var requiredPlayers = readyService.Players.Count();
        var playersReady = readyService.ReadyPlayers.Count();
        
        var trans = manialinks.CreateTransaction();
        var allPlayers = await playerManager.GetOnlinePlayersAsync();
        
        foreach (var player in allPlayers)
        {
            var isReady = readyService.ReadyPlayers.Any(p => p.AccountId == player.AccountId);
            var showButton = readyService.Players.Any(p => p.AccountId == player.AccountId);
            
            logger.LogDebug("Showing ReadyWidget for player (isReady: {IsReady}, showButton: {ShowButton})", isReady, showButton);

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
        
        var isReady = readyService.ReadyPlayers.Any(p => p.AccountId == player.AccountId);
        var showButton = readyService.Players.Any(p => p.AccountId == player.AccountId);

        await manialinks.SendManialinkAsync(player, "MatchReadyModule.ReadyWidget", new
        {
            isReady, requiredPlayers, playersReady, showButton
        });
    }

    public async Task UpdateWidgetAsync()
    {
        var readyPlayersCount = readyService.Players.Count();
        var allPlayers = await playerManager.GetOnlinePlayersAsync();
        
        var trans = manialinks.CreateTransaction();

        foreach (var player in allPlayers)
        {
            var isReady = readyService.ReadyPlayers.Any(p => p.AccountId == player.AccountId);

            logger.LogDebug("Updating ReadyWidget for player (isReady: {IsReady})", isReady);
            await trans.SendManialinkAsync(player, "MatchReadyModule.UpdateWidget", new { readyPlayersCount, isReady });
        }
        
        await trans.CommitAsync();
    }

    public Task HideWidgetAsync() => manialinks.HideManialinkAsync("MatchReadyModule.ReadyWidget");
}
