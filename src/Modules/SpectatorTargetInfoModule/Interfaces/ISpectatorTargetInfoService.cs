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

    public Task<IOnlinePlayer?> SetSpectatorTargetAsync(string spectatorLogin, string targetLogin);
    
    public Task SetSpectatorTargetAndSendAsync(string spectatorLogin, string targetLogin);

    public Task RemovePlayerFromSpectatorsListAsync(string spectatorLogin);

    public IEnumerable<string> GetLoginsOfPlayersSpectatingTarget(IOnlinePlayer targetPlayer);

    public SpectatorInfo ParseSpectatorStatus(int spectatorStatus);

    public int GetTimeDifference(CheckpointData leadingCheckpointData, CheckpointData targetCheckpointData);

    public int GetTimeDifference(int leadingCheckpointTime, int targetCheckpointTime);

    public string GetTeamColorAsync(PlayerTeam team);

    public int GetLastCheckpointIndexOfPlayer(IOnlinePlayer playerLogin);
    
    public Dictionary<int, CheckpointsGroup> GetCheckpointTimes();

    public Task<TmTeamInfo> GetTeamInfoAsync(PlayerTeam team);

    public Task ResetWidgetForSpectatorsAsync();

    public Task SendSpectatorInfoWidgetAsync(string spectatorLogin, IOnlinePlayer targetPlayer);

    public Task SendSpectatorInfoWidgetAsync(IEnumerable<string> spectatorLogins, IOnlinePlayer targetPlayer, int targetPlayerRank, int timeDifference);

    public Task SendSpectatorInfoWidgetAsync(string spectatorLogin, IOnlinePlayer targetPlayer, int targetPlayerRank, int timeDifference);

    public Task HideSpectatorInfoWidgetAsync();

    public Task HideSpectatorInfoWidgetAsync(string playerLogin);

    public Task SendReportSpectatorTargetManialinkAsync();

    public Task UpdateTeamInfoAsync();
    
    public Task UpdateIsTeamsModeAsync();

    public Task HideGameModeUiAsync();

    public Task AddFakePlayerAsync(); //TODO: remove
}
