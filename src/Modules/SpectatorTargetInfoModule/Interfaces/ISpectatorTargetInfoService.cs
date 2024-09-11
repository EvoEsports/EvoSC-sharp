using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Models;
using GbxRemoteNet.Structs;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Interfaces;

public interface ISpectatorTargetInfoService
{
    public Task InitializeAsync();
    
    public Task<IOnlinePlayer> GetOnlinePlayerByLoginAsync(string playerLogin);

    public Task AddCheckpointAsync(string playerLogin, int checkpointIndex, int checkpointTime);

    public Task ClearCheckpointsAsync();

    public Task<string?> GetLoginOfDedicatedPlayerAsync(int targetPlayerIdDedicated);

    public Task SetSpectatorTargetLoginAsync(string spectatorLogin, string targetLogin);

    public Task RemovePlayerFromSpectatorsListAsync(string spectatorLogin);

    public IEnumerable<string> GetLoginsOfPlayersSpectatingTarget(string targetPlayerLogin);

    public SpectatorInfo ParseSpectatorStatus(int spectatorStatus);

    public int GetTimeDifference(CheckpointData leadingCheckpointData, CheckpointData targetCheckpointData);

    public int GetTimeDifference(int leadingCheckpointTime, int targetCheckpointTime);

    public string GetTeamColorAsync(PlayerTeam team);

    public int GetLastCheckpointIndexOfPlayer(string playerLogin);

    public Dictionary<int, CheckpointsGroup> GetCheckpointTimes();

    public Task<TmTeamInfo> GetTeamInfoAsync(PlayerTeam team);

    public Task ResetWidgetForSpectatorsAsync();

    public Task SendWidgetAsync(IEnumerable<string> playerLogins, IOnlinePlayer targetPlayer, int targetPlayerRank, int timeDifference);

    public Task SendWidgetAsync(string playerLogin, IOnlinePlayer targetPlayer, int targetPlayerRank, int timeDifference);

    public Task HideWidgetAsync();

    public Task HideWidgetAsync(string playerLogin);

    public Task SendRequestTargetManialinkAsync();

    public Task UpdateTeamInfoAsync();
    
    public Task UpdateIsTeamsModeAsync();

    public Task HideGameModeUiAsync();

    public Task AddFakePlayerAsync();
}
