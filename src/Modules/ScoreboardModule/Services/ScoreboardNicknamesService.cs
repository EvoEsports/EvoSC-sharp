using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.ScoreboardModule.Interfaces;

namespace EvoSC.Modules.Official.ScoreboardModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class ScoreboardNicknamesService(
    IPlayerManagerService playerManagerService,
    IManialinkManager manialinkManager
) : IScoreboardNicknamesService
{
    private readonly Dictionary<string, string> _nicknames = new();

    public async Task AddNicknameByLoginAsync(string login)
    {
        var player = await playerManagerService.GetOnlinePlayerAsync(PlayerUtils.ConvertLoginToAccountId(login));

        if (player.NickName == player.UbisoftName)
        {
            return;
        }

        _nicknames[login] = player.NickName;
    }

    public Task ClearNicknamesAsync()
    {
        _nicknames.Clear();

        return Task.CompletedTask;
    }

    public async Task LoadNicknamesAsync()
    {
        var onlinePlayers = await playerManagerService.GetOnlinePlayersAsync();
        foreach (var player in onlinePlayers.Where(player => player.NickName != player.UbisoftName))
        {
            _nicknames[player.GetLogin()] = player.NickName;
        }
    }

    public Task SendNicknamesManialinkAsync() =>
        manialinkManager.SendPersistentManialinkAsync("ScoreboardModule.PlayerNicknames",
            new { nicknames = ToManiaScriptArray(_nicknames) });

    public string ToManiaScriptArray(Dictionary<string, string> nicknameMap)
    {
        var entriesList = nicknameMap.Select(ToManiaScriptArrayEntry).ToList();
        var joinedEntries = string.Join(",\n", entriesList);

        return $"[{joinedEntries}]";
    }

    public string ToManiaScriptArrayEntry(KeyValuePair<string, string> loginNickname)
    {
        return $"\"{loginNickname.Key}\" => \"{EscapeNickname(loginNickname.Value)}\"";
    }

    public string EscapeNickname(string nickname)
    {
        return nickname.Replace("-->", "-\u2192", StringComparison.OrdinalIgnoreCase)
            .Replace("\"", "\\\"", StringComparison.OrdinalIgnoreCase);
    }
}
