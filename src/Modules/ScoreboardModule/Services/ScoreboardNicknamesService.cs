using System.Collections.Concurrent;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.ScoreboardModule.Interfaces;
using EvoSC.Modules.Official.ScoreboardModule.Utils;

namespace EvoSC.Modules.Official.ScoreboardModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class ScoreboardNicknamesService(IPlayerManagerService playerManagerService, IManialinkManager manialinkManager)
    : IScoreboardNicknamesService
{
    private readonly ConcurrentDictionary<string, string> _nicknames = new();

    public async Task InitializeNicknamesAsync()
    {
        var onlinePlayers = await playerManagerService.GetOnlinePlayersAsync();
        foreach (var player in onlinePlayers.Where(player => player.NickName != player.UbisoftName))
        {
            _nicknames[player.GetLogin()] = player.NickName;
        }
    }

    public async Task FetchAndAddNicknameByLoginAsync(string login)
    {
        var player = await playerManagerService.GetOnlinePlayerAsync(PlayerUtils.ConvertLoginToAccountId(login));

        if (player.NickName == player.UbisoftName)
        {
            return;
        }

        _nicknames[login] = player.NickName;
    }

    public Task<ConcurrentDictionary<string, string>> GetNicknamesAsync()
    {
        return Task.FromResult(_nicknames);
    }

    public Task ClearNicknamesAsync()
    {
        _nicknames.Clear();

        return Task.CompletedTask;
    }

    public Task SendNicknamesManialinkAsync() =>
        manialinkManager.SendPersistentManialinkAsync("ScoreboardModule.PlayerNicknames",
            new { nicknames = ScoreboardNicknameUtils.ToManiaScriptArray(_nicknames) });
}
