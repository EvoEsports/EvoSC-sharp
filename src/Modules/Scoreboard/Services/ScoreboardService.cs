using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.Scoreboard.Interfaces;

namespace EvoSC.Modules.Official.Scoreboard.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class ScoreboardService : IScoreboardService
{
    private readonly IManialinkManager _manialinks;
    private readonly Locale _locale;
    private readonly IPlayerManagerService _playerManager;

    public ScoreboardService(IManialinkManager manialinks, Locale locale, IPlayerManagerService playerManager)
    {
        _manialinks = manialinks;
        _locale = locale;
        _playerManager = playerManager;
    }

    public async Task ShowScoreboard(string playerLogin)
    {
        await _manialinks.SendManialinkAsync(
            await _playerManager.GetPlayerAsync(PlayerUtils.ConvertLoginToAccountId(playerLogin)),
            "Scoreboard.Scoreboard",
            new { Locale = _locale, MaxPlayers = 64 });
    }
}
