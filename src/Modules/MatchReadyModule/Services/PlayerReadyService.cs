using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;

namespace EvoSC.Modules.Official.MatchReadyModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class PlayerReadyService : IPlayerReadyService
{
    private readonly IPlayerReadyTrackerService _playerReadyTrackerService;
    private readonly IManialinkManager _manialinks;
    private readonly IServerClient _server;
    private readonly IPlayerManagerService _players;
    
    private bool _widgetEnabled;
    private readonly object _widgetEnabledLock = new();

    public PlayerReadyService(IPlayerReadyTrackerService playerReadyTrackerService, IManialinkManager manialinks,
        IServerClient server, IPlayerManagerService players)
    {
        _playerReadyTrackerService = playerReadyTrackerService;
        _manialinks = manialinks;
        _server = server;
        _players = players;
    }

    public bool MatchIsStarted => _playerReadyTrackerService.MatchStarted;

    public async Task SetPlayerReadyStatusAsync(IPlayer player, bool isReady)
    {
        if (!_playerReadyTrackerService.RequiredPlayers.Contains(player))
        {
            return;
        }
        
        await _playerReadyTrackerService.SetIsReadyAsync(player, isReady);

        if (isReady)
        {
            await _server.InfoMessageAsync($"$<{player.NickName}$> is ready.");
        }
        else
        {
            await _server.InfoMessageAsync($"$<{player.NickName}$> is no longer ready.");
        }

        await UpdateWidgetAsync();
    }

    public Task ResetReadyWidgetAsync(bool resetRequired)
    {
        _playerReadyTrackerService.Reset(resetRequired);
        return Task.CompletedTask;
    }

    public Task SetMatchStartedAsync()
    {
        _playerReadyTrackerService.SetMatchStarted(true);
        return Task.CompletedTask;
    }

    public Task<bool> PlayerIsReadyAsync(IPlayer player) =>
        Task.FromResult(_playerReadyTrackerService.ReadyPlayers.Any(p => p.AccountId == player.AccountId));

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
        var isReady = _playerReadyTrackerService.ReadyPlayers.Contains(player);
        var requiredPlayers = _playerReadyTrackerService.RequiredPlayers.Count();
        var playersReady = _playerReadyTrackerService.ReadyPlayers.Count();
        var showButton = _playerReadyTrackerService.RequiredPlayers.Any(p => p.AccountId == player.AccountId);

        await _manialinks.SendManialinkAsync(player, "MatchReadyModule.ReadyWidget",
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
        
        foreach (var player in await _players.GetOnlinePlayersAsync())
        {
            await _manialinks.SendManialinkAsync(player, "MatchReadyModule.UpdateWidget",
                new
                {
                    PlayerCount = _playerReadyTrackerService.ReadyPlayers.Count(),
                    IsReady = _playerReadyTrackerService.ReadyPlayers.Contains(player)
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
            await _manialinks.HideManialinkAsync("MatchReadyModule.ReadyWidget");
            await _manialinks.HideManialinkAsync("MatchReadyModule.UpdateWidget");
        }
    }

    public async Task AddRequiredPlayers(params IPlayer[] players)
    {
        foreach (var player in players)
        {
            await _playerReadyTrackerService.AddRequiredPlayerAsync(player);
        }
    }
}
