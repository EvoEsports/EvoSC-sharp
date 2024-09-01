using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Config;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Interfaces;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Models;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class SpectatorTargetInfoService(
    IManialinkManager manialinks,
    IServerClient server,
    IPlayerManagerService playerManagerService,
    ISpectatorTargetInfoSettings settings
) : ISpectatorTargetInfoService
{
    private const string WidgetTemplate = "SpectatorTargetInfoModule.SpectatorTargetInfo";

    private readonly Dictionary<int, List<CheckpointData>> _checkpointTimes = new(); // cp-id -> data
    private readonly Dictionary<string, string> _spectatorTargets = new(); // login -> login

    public async Task AddCheckpointAsync(string playerLogin, int checkpointIndex, int checkpointTime)
    {
        var player = await playerManagerService.GetOnlinePlayerAsync(PlayerUtils.ConvertLoginToAccountId(playerLogin));

        if (!_checkpointTimes.TryGetValue(checkpointIndex, out var value))
        {
            value = [];
            _checkpointTimes.Add(checkpointIndex, value);
        }

        value.Add(new CheckpointData(player, checkpointTime));
        _checkpointTimes[checkpointIndex] = value.OrderBy(cpData => cpData.time).ToList();

        // var spectatorLoginsWatchingPlayer = new List<string> { playerLogin };
        // await UpdateWidgetAsync(
        //     spectatorLoginsWatchingPlayer,
        //     _checkpointTimes[checkpointIndex].First(),
        //     newCheckpointData,
        //     GetRankFromCheckpointList(_checkpointTimes[checkpointIndex], playerLogin)
        // );
    }

    public Task ClearCheckpointsAsync()
    {
        _checkpointTimes.Clear();

        return Task.CompletedTask;
    }

    public async Task<string?> GetLoginOfDedicatedPlayerAsync(int targetPlayerIdDedicated)
    {
        var serverPlayers = await server.Remote.GetPlayerListAsync();

        return serverPlayers.Where(player => player.PlayerId == targetPlayerIdDedicated)
            .Select(player => player.Login)
            .FirstOrDefault();
    }

    public async Task UpdateSpectatorTargetAsync(string spectatorLogin, int targetPlayerIdDedicated)
    {
        var targetLogin = await GetLoginOfDedicatedPlayerAsync(targetPlayerIdDedicated);

        if (targetLogin == null)
        {
            await RemovePlayerFromSpectatorsListAsync(spectatorLogin);
            return;
        }

        await UpdateSpectatorTargetAsync(spectatorLogin, targetLogin);
    }

    public Task UpdateSpectatorTargetAsync(string spectatorLogin, string targetLogin)
    {
        _spectatorTargets[spectatorLogin] = targetLogin;
        //TODO: update manialink for user(s)

        return Task.CompletedTask;
    }

    public async Task RemovePlayerFromSpectatorsListAsync(string spectatorLogin)
    {
        _spectatorTargets.Remove(spectatorLogin);
        await manialinks.HideManialinkAsync(spectatorLogin);
    }

    public IEnumerable<string> GetLoginsSpectatingTarget(string targetPlayerLogin)
    {
        return _spectatorTargets.Where(specTarget => specTarget.Value == targetPlayerLogin)
            .Select(specTarget => specTarget.Key);
    }

    public async Task UpdateWidgetAsync(List<string> playerLogins, CheckpointData leadingCheckpointData,
        CheckpointData targetCheckpointData, int targetPlayerRank)
    {
        var timeDifference = GetTimeDifference(leadingCheckpointData, targetCheckpointData);

        foreach (var spectatorLogin in playerLogins)
        {
            await manialinks.SendManialinkAsync(spectatorLogin, WidgetTemplate,
                new
                {
                    settings,
                    timeDifference,
                    playerRank = targetPlayerRank,
                    playerName = targetCheckpointData.player.NickName
                });
        }
    }

    public SpectatorInfo ParseSpectatorStatus(int spectatorStatus)
    {
        return new SpectatorInfo(
            Convert.ToBoolean(spectatorStatus % 10),
            Convert.ToBoolean((spectatorStatus / 10) % 10),
            Convert.ToBoolean((spectatorStatus / 100) % 10),
            Convert.ToBoolean((spectatorStatus / 1000) % 10),
            spectatorStatus / 10_000
        );
    }

    public int GetRankFromCheckpointList(List<CheckpointData> sortedCheckpointTimes, string targetPlayerLogin)
    {
        var rank = 1;
        foreach (var checkpointData in sortedCheckpointTimes)
        {
            if (checkpointData.player.GetLogin() == targetPlayerLogin) return rank;
            rank++;
        }

        return -1;
    }

    public int GetTimeDifference(CheckpointData leadingCheckpointData, CheckpointData targetCheckpointData)
    {
        return GetTimeDifference(leadingCheckpointData.time, targetCheckpointData.time);
    }

    public int GetTimeDifference(int leadingCheckpointTime, int targetCheckpointTime)
    {
        return targetCheckpointTime - leadingCheckpointTime;
    }

    public Task<Dictionary<int, List<CheckpointData>>> GetCheckpointTimesAsync()
    {
        return Task.FromResult(_checkpointTimes);
    }

    public async Task SendManiaLinkAsync() =>
        await manialinks.SendManialinkAsync(WidgetTemplate, new { settings });


    public async Task SendManiaLinkAsync(string playerLogin)
    {
        await manialinks.SendManialinkAsync(playerLogin, WidgetTemplate, new { settings });
    }

    public async Task HideManiaLinkAsync() =>
        await manialinks.HideManialinkAsync(WidgetTemplate);

    public Task HideNadeoSpectatorInfoAsync()
    {
        var hudSettings = new List<string>
        {
            @"{""uimodules"": [
                {
                    ""id"": ""Race_SpectatorBase_Name"",
                    ""visible"": false,
                    ""visible_update"": true,
                },
                {
                    ""id"": ""Race_SpectatorBase_Commands"",
                    ""visible"": true,
                    ""visible_update"": true,
                }
            ]}"
        };

        return server.Remote.TriggerModeScriptEventArrayAsync("Common.UIModules.SetProperties", hudSettings.ToArray());
    }

    public Task ShowNadeoSpectatorInfoAsync()
    {
        var hudSettings = new List<string>
        {
            @"{""uimodules"": [
                {
                    ""id"": ""Race_SpectatorBase_Name"",
                    ""visible"": true,
                    ""visible_update"": true,
                },
                {
                    ""id"": ""Race_SpectatorBase_Commands"",
                    ""visible"": true,
                    ""visible_update"": true,
                }
            ]}"
        };

        return server.Remote.TriggerModeScriptEventArrayAsync("Common.UIModules.SetProperties", hudSettings.ToArray());
    }

    public Task ForwardCheckpointTimeToClientsAsync(WayPointEventArgs wayPointEventArgs)
    {
        return manialinks.SendManialinkAsync("SpectatorTargetInfoModule.NewCpTime",
            new
            {
                accountId = wayPointEventArgs.AccountId,
                time = wayPointEventArgs.RaceTime,
                cpIndex = wayPointEventArgs.CheckpointInRace
            });
    }

    public Task ResetCheckpointTimesAsync()
    {
        manialinks.HideManialinkAsync("SpectatorTargetInfoModule.NewCpTime");
        return manialinks.SendManialinkAsync("SpectatorTargetInfoModule.ResetCpTimes");
    }

    public Task ForwardDnfToClientsAsync(PlayerUpdateEventArgs playerUpdateEventArgs)
    {
        return manialinks.SendManialinkAsync("SpectatorTargetInfoModule.NewCpTime",
            new { accountId = playerUpdateEventArgs.AccountId, time = 0, cpIndex = -1 });
    }

    public async Task AddFakePlayerAsync()
    {
        await server.Remote.ConnectFakePlayerAsync();
    }
}
