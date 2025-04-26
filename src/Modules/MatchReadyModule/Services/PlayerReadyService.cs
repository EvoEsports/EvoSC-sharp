using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;

namespace EvoSC.Modules.Official.MatchReadyModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class PlayerReadyService(IPlayerReadyTrackerService playerReadyTrackerService, IManialinkManager manialinks,
        IChatService chat, IPlayerManagerService players)
    : IPlayerReadyService
{
    private bool _widgetEnabled;
    private readonly object _widgetEnabledLock = new();

    public bool MatchIsStarted => playerReadyTrackerService.MatchStarted;

    public async Task SetPlayerReadyStatusAsync(IPlayer player, bool isReady)
    {
        if (!playerReadyTrackerService.RequiredPlayers.Contains(player))
        {
            return;
        }
        
        await playerReadyTrackerService.SetIsReadyAsync(player, isReady);

        if (isReady)
        {
            await chat.InfoMessageAsync($"$<{player.NickName}$> is ready.");
        }
        else
        {
            await chat.InfoMessageAsync($"$<{player.NickName}$> is no longer ready.");
        }

        await UpdateWidgetAsync();
    }

    public Task ResetReadyWidgetAsync(bool resetRequired)
    {
        playerReadyTrackerService.Reset(resetRequired);
        return Task.CompletedTask;
    }

    public Task SetMatchStartedAsync()
    {
        playerReadyTrackerService.SetMatchStarted(true);
        return Task.CompletedTask;
    }

    public Task<bool> PlayerIsReadyAsync(IPlayer player) =>
        Task.FromResult(playerReadyTrackerService.ReadyPlayers.Any(p => p.AccountId == player.AccountId));

    public async Task SendWidgetAsync(IPlayer player)
    {
        lock (_widgetEnabledLock)
        {
            if (!_widgetEnabled)
            {
                return;
            }
        }
        
        // var isReady = await PlayerIsReadyAsync(player);
        var isReady = playerReadyTrackerService.ReadyPlayers.Contains(player);
        var requiredPlayers = playerReadyTrackerService.RequiredPlayers.Count();
        var playersReady = playerReadyTrackerService.ReadyPlayers.Count();
        var showButton = playerReadyTrackerService.RequiredPlayers.Any(p => p.AccountId == player.AccountId);

        await manialinks.SendManialinkAsync(player, "MatchReadyModule.ReadyWidget",
            new { isReady, requiredPlayers, playersReady, showButton });
    }

    public async Task UpdateWidgetAsync()
    {
        lock (_widgetEnabledLock)
        {
            if (!_widgetEnabled)
            {
                return;
            }
        }
        
        foreach (var player in await players.GetOnlinePlayersAsync())
        {
            await manialinks.SendManialinkAsync(player, "MatchReadyModule.UpdateWidget",
                new
                {
                    PlayerCount = playerReadyTrackerService.ReadyPlayers.Count(),
                    IsReady = playerReadyTrackerService.ReadyPlayers.Contains(player)
                });
        }
    }

    public async Task SetWidgetEnabled(bool enabled)
    {
        lock (_widgetEnabledLock)
        {
            _widgetEnabled = enabled;
        }

        if (!enabled)
        {
            await manialinks.HideManialinkAsync("MatchReadyModule.ReadyWidget");
            await manialinks.HideManialinkAsync("MatchReadyModule.UpdateWidget");
        }
    }

    public async Task AddRequiredPlayers(params IPlayer[] players)
    {
        foreach (var player in players)
        {
            await playerReadyTrackerService.AddRequiredPlayerAsync(player);
        }
    }
}
