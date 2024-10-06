using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.GameModeUiModule.Enums;
using EvoSC.Modules.Official.GameModeUiModule.Interfaces;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Config;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Interfaces;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Models;
using GbxRemoteNet.Structs;
using LinqToDB.Common;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class SpectatorTargetInfoService(
    IManialinkManager manialinks,
    IServerClient server,
    IPlayerManagerService playerManagerService,
    IMatchSettingsService matchSettingsService,
    ISpectatorTargetInfoSettings settings,
    IThemeManager theme,
    IGameModeUiModuleService gameModeUiModuleService,
    ILogger<SpectatorTargetInfoService> logger
) : ISpectatorTargetInfoService
{
    private const string ReportTargetTemplate = "SpectatorTargetInfoModule.ReportSpecTarget";
    private const string WidgetTemplate = "SpectatorTargetInfoModule.SpectatorTargetInfo";

    private readonly object _checkpointTimesMutex = new();
    private readonly object _spectatorTargetsMutex = new();
    private readonly Dictionary<int, CheckpointsGroup> _checkpointTimes = new(); // cp-id -> CheckpointsGroup
    private readonly Dictionary<string, IOnlinePlayer> _spectatorTargets = new(); // login -> IOnlinePlayer
    private readonly Dictionary<PlayerTeam, TmTeamInfo> _teamInfos = new();
    private bool _isTeamsMode;

    public async Task InitializeAsync()
    {
        await UpdateIsTeamsModeAsync();
        await FetchAndCacheTeamInfoAsync();
        await SendReportSpectatorTargetManialinkAsync();
        await HideGameModeUiAsync();
    }

    public Task<IOnlinePlayer> GetOnlinePlayerByLoginAsync(string playerLogin)
        => playerManagerService.GetOnlinePlayerAsync(PlayerUtils.ConvertLoginToAccountId(playerLogin));

    public async Task AddCheckpointAsync(string playerLogin, int checkpointIndex, int checkpointTime)
    {
        var player = await GetOnlinePlayerByLoginAsync(playerLogin);
        var newCheckpointData = new CheckpointData(player, checkpointTime);
        CheckpointsGroup checkpointsGroup = [];

        lock (_checkpointTimesMutex)
        {
            if (_checkpointTimes.TryGetValue(checkpointIndex, out var existingCheckpointGroup))
            {
                checkpointsGroup = existingCheckpointGroup;
            }

            checkpointsGroup.Add(newCheckpointData);
            _checkpointTimes[checkpointIndex] = checkpointsGroup;
        }

        var spectatorLogins = GetLoginsOfPlayersSpectatingTarget(player).ToList();
        if (spectatorLogins.IsNullOrEmpty())
        {
            return;
        }

        var leadingCheckpointData = checkpointsGroup.First();
        var timeDifference = GetTimeDifference(leadingCheckpointData.time, newCheckpointData.time);

        await SendSpectatorInfoWidgetAsync(
            spectatorLogins,
            player,
            checkpointsGroup.GetRank(playerLogin),
            timeDifference
        );
    }

    public Task ClearCheckpointsAsync()
    {
        lock (_checkpointTimesMutex)
        {
            _checkpointTimes.Clear();
        }

        return Task.CompletedTask;
    }

    public async Task<string?> GetLoginOfDedicatedPlayerAsync(int targetPlayerIdDedicated)
    {
        var serverPlayers = await server.Remote.GetPlayerListAsync();

        return serverPlayers.Where(player => player.PlayerId == targetPlayerIdDedicated)
            .Select(player => player.Login)
            .FirstOrDefault();
    }

    public async Task<IOnlinePlayer?> SetSpectatorTargetAsync(string spectatorLogin, string targetLogin)
    {
        if (spectatorLogin == targetLogin)
        {
            return null; //Can't spec yourself
        }

        var targetPlayer = await GetOnlinePlayerByLoginAsync(targetLogin);

        lock (_spectatorTargetsMutex)
        {
            if (_spectatorTargets.TryGetValue(spectatorLogin, out var target) && target == targetPlayer)
            {
                return null; //Player is already spectating target
            }

            _spectatorTargets[spectatorLogin] = targetPlayer;
        }

        logger.LogTrace("Updated spectator target {spectatorLogin} -> {targetLogin}.", spectatorLogin,
            targetLogin);

        return targetPlayer;
    }

    public async Task SetSpectatorTargetAndSendAsync(string spectatorLogin, string targetLogin)
    {
        var targetPlayer = await SetSpectatorTargetAsync(spectatorLogin, targetLogin);
        if (targetPlayer != null)
        {
            await SendSpectatorInfoWidgetAsync(spectatorLogin, targetPlayer);
        }
    }

    public Task RemovePlayerFromSpectatorsListAsync(string spectatorLogin)
    {
        lock (_spectatorTargetsMutex)
        {
            if (_spectatorTargets.Remove(spectatorLogin))
            {
                logger.LogTrace("Removed spectator {spectatorLogin}.", spectatorLogin);
            }
        }

        return Task.CompletedTask;
    }

    public IEnumerable<string> GetLoginsOfPlayersSpectatingTarget(IOnlinePlayer targetPlayer)
    {
        Dictionary<string, IOnlinePlayer> spectatorTargets;

        lock (_spectatorTargetsMutex)
        {
            spectatorTargets = _spectatorTargets;
        }

        return spectatorTargets.Where(specTarget => specTarget.Value.AccountId == targetPlayer.AccountId)
            .Select(specTarget => specTarget.Key);
    }

    public int GetTimeDifference(int leadingCheckpointTime, int targetCheckpointTime)
    {
        return targetCheckpointTime - leadingCheckpointTime;
    }

    public string GetTeamColor(PlayerTeam team)
    {
        return _isTeamsMode ? _teamInfos[team].RGB : (string)theme.Theme.UI_AccentPrimary;
    }

    public int GetLastCheckpointIndexOfPlayer(IOnlinePlayer player)
    {
        var playerLogin = player.GetLogin();
        foreach (var (checkpointIndex, checkpointsGroup) in GetCheckpointTimes().Reverse())
        {
            if (checkpointsGroup.GetPlayerCheckpointData(playerLogin) != null)
            {
                return checkpointIndex;
            }
        }

        return -1;
    }

    public Dictionary<int, CheckpointsGroup> GetCheckpointTimes()
    {
        lock (_checkpointTimesMutex)
        {
            return _checkpointTimes;
        }
    }

    public async Task ResetWidgetForSpectatorsAsync()
    {
        Dictionary<string, IOnlinePlayer> spectatorTargets;

        lock (_spectatorTargetsMutex)
        {
            spectatorTargets = _spectatorTargets;
        }

        foreach (var (spectatorLogin, targetPlayer) in spectatorTargets)
        {
            var widgetData = GetWidgetData(targetPlayer, 1, 0);
            await SendSpectatorInfoWidgetAsync(spectatorLogin, targetPlayer, widgetData);
        }
    }

    public async Task SendSpectatorInfoWidgetAsync(string spectatorLogin, IOnlinePlayer targetPlayer)
    {
        var targetLogin = targetPlayer.GetLogin();
        var checkpointIndex = GetLastCheckpointIndexOfPlayer(targetPlayer);
        var targetRank = 1;
        var timeDifference = 0;

        if (GetCheckpointTimes().TryGetValue(checkpointIndex, out var checkpointsGroup))
        {
            var leadingCpData = checkpointsGroup.First();
            var targetCpData = checkpointsGroup.GetPlayerCheckpointData(targetLogin);
            targetRank = checkpointsGroup.GetRank(targetLogin);
            timeDifference = GetTimeDifference(leadingCpData.time, targetCpData?.time ?? 0);
        }

        var widgetData = GetWidgetData(targetPlayer, targetRank, timeDifference);
        await SendSpectatorInfoWidgetAsync(spectatorLogin, targetPlayer, widgetData);
    }

    public async Task SendSpectatorInfoWidgetAsync(IEnumerable<string> spectatorLogins, IOnlinePlayer targetPlayer,
        int targetPlayerRank, int timeDifference)
    {
        var widgetData = GetWidgetData(targetPlayer, targetPlayerRank, timeDifference);
        foreach (var spectatorLogin in spectatorLogins)
        {
            await SendSpectatorInfoWidgetAsync(spectatorLogin, targetPlayer, widgetData);
        }
    }

    public Task SendSpectatorInfoWidgetAsync(string spectatorLogin, IOnlinePlayer targetPlayer, object widgetData) =>
        manialinks.SendManialinkAsync(spectatorLogin, WidgetTemplate, widgetData);

    public object GetWidgetData(IOnlinePlayer player, int rank, int timeDifference)
    {
        return new
        {
            settings,
            timeDifference,
            playerRank = rank,
            playerName = player.NickName,
            playerTeam = player.Team,
            playerLogin = player.GetLogin(),
            teamColorCode = new ColorUtils().Opacity(GetTeamColor(player.Team), 80)
        };
    }

    public Task HideSpectatorInfoWidgetAsync()
        => manialinks.HideManialinkAsync(WidgetTemplate);

    public Task HideSpectatorInfoWidgetAsync(string playerLogin)
        => manialinks.HideManialinkAsync(playerLogin, WidgetTemplate);

    public Task SendReportSpectatorTargetManialinkAsync() =>
        manialinks.SendPersistentManialinkAsync(ReportTargetTemplate);

    public async Task FetchAndCacheTeamInfoAsync()
    {
        _teamInfos[PlayerTeam.Team1] = await server.Remote.GetTeamInfoAsync((int)PlayerTeam.Team1 + 1);
        _teamInfos[PlayerTeam.Team2] = await server.Remote.GetTeamInfoAsync((int)PlayerTeam.Team2 + 1);
    }

    public async Task UpdateIsTeamsModeAsync()
    {
        _isTeamsMode =
            await matchSettingsService.GetCurrentModeAsync() is DefaultModeScriptName.Teams
                or DefaultModeScriptName.TmwtTeams;

        logger.LogInformation("Team mode is {state}", _isTeamsMode ? "active" : "not active");
    }

    public Task HideGameModeUiAsync() =>
        gameModeUiModuleService.ApplyComponentSettingsAsync(GameModeUiComponents.SpectatorBaseName, false, 0, 0, 1);
}
