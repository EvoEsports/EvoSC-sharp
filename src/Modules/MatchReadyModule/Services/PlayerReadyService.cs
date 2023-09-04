using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
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
    
    public PlayerReadyService(IPlayerReadyTrackerService playerReadyTrackerService, IManialinkManager manialinks, IServerClient server)
    {
        _playerReadyTrackerService = playerReadyTrackerService;
        _manialinks = manialinks;
        _server = server;
    }

    public bool MatchIsStarted => _playerReadyTrackerService.MatchStarted;

    public async Task SetPlayerReadyStatusAsync(IPlayer player, bool isReady)
    {
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
        var isReady = await PlayerIsReadyAsync(player);
        var requiredPlayers = _playerReadyTrackerService.RequiredPlayers.Count();
        var playersReady = _playerReadyTrackerService.ReadyPlayers.Count();

        await _manialinks.SendManialinkAsync(player, "MatchReadyModule.ReadyWidget",
            new { isReady, requiredPlayers, playersReady });
    }

    public async Task UpdateWidgetAsync()
    {
        foreach (var player in _playerReadyTrackerService.RequiredPlayers)
        {
            await _manialinks.SendManialinkAsync("MatchReadyModule.UpdateWidget",
                new
                {
                    PlayerCount = _playerReadyTrackerService.ReadyPlayers.Count(),
                    IsReady = await PlayerIsReadyAsync(player)
                });
        }
    }
}
