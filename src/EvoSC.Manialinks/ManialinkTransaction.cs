using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Util;
using GbxRemoteNet;
using GbxRemoteNet.Interfaces;

namespace EvoSC.Manialinks;

public class ManialinkTransaction(
    IManialinkManager manialinkManager,
    IServerClient server,
    IPlayerManagerService playerManagerService
) : IManialinkTransaction
{
    private readonly MultiCall _serverCalls = new();
    private readonly HashSet<string> _persistentRemovals = [];

    public async Task SendManialinkAsync(string name, IDictionary<string, object?> data)
    {
        var onlinePlayers = await playerManagerService.GetOnlinePlayersAsync();
        
        await SendManialinkAsync(onlinePlayers, name, data);
    }

    public async Task SendManialinkAsync(string name, dynamic data)
    {
        var onlinePlayers = await playerManagerService.GetOnlinePlayersAsync();
        
        await SendManialinkAsync(onlinePlayers, name, data);
    }

    public Task SendManialinkAsync(string name) => SendManialinkAsync(name, new { });

    public Task SendManialinkAsync(IPlayer player, string name, IDictionary<string, object?> data) =>
        AddSendCallAsync(player, name, data);

    public Task SendManialinkAsync(IPlayer player, string name, dynamic data) => AddSendCallAsync(player, name, data);

    public async Task SendManialinkAsync(string playerLogin, string name, dynamic data)
    {
        var player = await playerManagerService.GetOnlinePlayerAsync(PlayerUtils.ConvertLoginToAccountId(playerLogin));

        await AddSendCallAsync(player, name, data);
    }

    public async Task SendManialinkAsync(IEnumerable<IPlayer> players, string name, IDictionary<string, object?> data)
    {
        var effectiveName = manialinkManager.GetEffectiveName(name);
        var playerToSendTo = players.Where(p => !p.ManialinkIsHidden(effectiveName));
        var output = await GetSendOutputAsync(name, data);

        foreach (var player in playerToSendTo)
        {
            _serverCalls.Add(nameof(IGbxRemoteClient.SendDisplayManialinkPageToLoginAsync), player.GetLogin(), output,
                0, false);
        }
    }

    public async Task SendManialinkAsync(IEnumerable<IPlayer> players, string name, dynamic data)
    {
        var effectiveName = manialinkManager.GetEffectiveName(name);
        var playerToSendTo = players.Where(p => !p.ManialinkIsHidden(effectiveName));
        var output = await GetSendOutputAsync(name, data);

        foreach (var player in playerToSendTo)
        {
            _serverCalls.Add(nameof(IGbxRemoteClient.SendDisplayManialinkPageToLoginAsync), player.GetLogin(), output,
                0, false);
        }
    }

    public Task HideManialinkAsync(string name) => AddHideCallAsync(name);

    public Task HideManialinkAsync(IPlayer player, string name) => AddHideCallAsync(player, name);

    public Task HideManialinkAsync(string playerLogin, string name) => AddHideCallAsync(playerLogin, name);

    public async Task HideManialinkAsync(IEnumerable<IPlayer> players, string name)
    {
        foreach (var player in players)
        {
            await AddHideCallAsync(player, name);
        }
    }

    private async Task AddSendCallAsync(string name, IDictionary<string, object?> data)
    {
        var output = await GetSendOutputAsync(name, data);
        _serverCalls.Add(nameof(IGbxRemoteClient.SendDisplayManialinkPageAsync), output, 0, false);
    }

    private async Task AddSendCallAsync(string name, dynamic data)
    {
        var output = await GetSendOutputAsync(name, data);
        _serverCalls.Add(nameof(IGbxRemoteClient.SendDisplayManialinkPageAsync), output, 0, false);
    }

    private async Task AddSendCallAsync(string playerLogin, string name, IDictionary<string, object?> data)
    {
        var player = await playerManagerService.GetOnlinePlayerAsync(PlayerUtils.ConvertLoginToAccountId(playerLogin));

        if (player.ManialinkIsHidden(manialinkManager.GetEffectiveName(name)))
        {
            return;
        }

        var output = await GetSendOutputAsync(name, data);
        _serverCalls.Add(nameof(IGbxRemoteClient.SendDisplayManialinkPageToLoginAsync), playerLogin, output, 0, false);
    }

    private async Task AddSendCallAsync(string playerLogin, string name, dynamic data)
    {
        var player = await playerManagerService.GetOnlinePlayerAsync(PlayerUtils.ConvertLoginToAccountId(playerLogin));

        await AddSendCallAsync(player, name, data);
    }

    private async Task AddSendCallAsync(IPlayer player, string name, IDictionary<string, object?> data)
    {
        if (player.ManialinkIsHidden(manialinkManager.GetEffectiveName(name)))
        {
            return;
        }

        var output = await GetSendOutputAsync(name, data);
        _serverCalls.Add(nameof(IGbxRemoteClient.SendDisplayManialinkPageToLoginAsync), player.GetLogin(), output, 0,
            false);
    }

    private async Task AddSendCallAsync(IPlayer player, string name, dynamic data)
    {
        if (player.ManialinkIsHidden(manialinkManager.GetEffectiveName(name)))
        {
            return;
        }

        var output = await GetSendOutputAsync(name, data);
        _serverCalls.Add(nameof(IGbxRemoteClient.SendDisplayManialinkPageToLoginAsync), player.GetLogin(), output, 0,
            false);
    }

    private Task AddHideCallAsync(string name)
    {
        var output = PrepareAndGetHideCall(name);
        _serverCalls.Add(nameof(IGbxRemoteClient.SendDisplayManialinkPageAsync), output, 3, true);
        return Task.CompletedTask;
    }

    private Task AddHideCallAsync(string playerLogin, string name)
    {
        var output = PrepareAndGetHideCall(name);
        _serverCalls.Add(nameof(IGbxRemoteClient.SendDisplayManialinkPageToLoginAsync), playerLogin, output, 3, true);
        return Task.CompletedTask;
    }

    private Task AddHideCallAsync(IPlayer player, string name) => AddHideCallAsync(player.GetLogin(), name);

    private async Task<string> GetSendOutputAsync(string name, IDictionary<string, object?> data)
    {
        return await manialinkManager.PrepareAndRenderAsync(manialinkManager.GetEffectiveName(name), data);
    }

    private async Task<string> GetSendOutputAsync(string name, dynamic data)
    {
        return await manialinkManager.PrepareAndRenderAsync(manialinkManager.GetEffectiveName(name), data);
    }

    private string PrepareAndGetHideCall(string name)
    {
        name = manialinkManager.GetEffectiveName(name);
        _persistentRemovals.Add(name);
        return ManialinkUtils.CreateHideManialink(name);
    }

    public Task CommitAsync() => server.Remote.MultiCallAsync(_serverCalls);
}
