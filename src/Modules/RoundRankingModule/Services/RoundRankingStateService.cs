using System.Collections.Concurrent;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.RoundRankingModule.Interfaces;
using EvoSC.Modules.Official.RoundRankingModule.Models;

namespace EvoSC.Modules.Official.RoundRankingModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class RoundRankingStateService : IRoundRankingStateService
{
    private readonly object _pointsRepartitionMutex = new();
    private readonly object _isTimeAttackModeMutex = new();
    private readonly object _isTeamsModeMutex = new();
    private readonly PointsRepartition _pointsRepartition = [];
    private readonly CheckpointsRepository _checkpointsRepository = new();
    private readonly ConcurrentDictionary<PlayerTeam, string> _teamColors = new();
    private bool _isTimeAttackMode;
    private bool _isTeamsMode;

    public Task<PointsRepartition> GetPointsRepartitionAsync()
    {
        lock (_pointsRepartitionMutex)
        {
            return Task.FromResult(_pointsRepartition);
        }
    }

    public Task UpdatePointsRepartitionAsync(string pointsRepartitionString)
    {
        lock (_pointsRepartitionMutex)
        {
            _pointsRepartition.Update(pointsRepartitionString);
        }

        return Task.CompletedTask;
    }

    public Task<CheckpointsRepository> GetRepositoryAsync()
    {
        return Task.FromResult(_checkpointsRepository);
    }

    public Task UpdateRepositoryEntryAsync(string accountId, CheckpointData checkpointData)
    {
        _checkpointsRepository[accountId] = checkpointData;

        return Task.CompletedTask;
    }

    public Task RemoveRepositoryEntryAsync(string accountId)
    {
        _checkpointsRepository.Remove(accountId, out var removedCheckpointData);

        return Task.CompletedTask;
    }

    public Task ClearRepositoryAsync()
    {
        _checkpointsRepository.Clear();

        return Task.CompletedTask;
    }

    public Task<ConcurrentDictionary<PlayerTeam, string>> GetTeamColorsAsync()
    {
        return Task.FromResult(_teamColors);
    }

    public Task SetTeamColorsAsync(string team1Color, string team2Color, string teamUnknownColor)
    {
        _teamColors[PlayerTeam.Team1] = team1Color;
        _teamColors[PlayerTeam.Team2] = team2Color;
        _teamColors[PlayerTeam.Unknown] = teamUnknownColor;

        return Task.CompletedTask;
    }

    public Task SetIsTeamsModeAsync(bool isTeamsMode)
    {
        lock (_isTeamsModeMutex)
        {
            _isTeamsMode = isTeamsMode;
        }

        return Task.CompletedTask;
    }

    public Task SetIsTimeAttackModeAsync(bool isTimeAttackMode)
    {
        lock (_isTimeAttackModeMutex)
        {
            _isTimeAttackMode = isTimeAttackMode;
        }

        return Task.CompletedTask;
    }

    public Task<bool> GetIsTeamsModeAsync()
    {
        lock (_isTeamsModeMutex)
        {
            return Task.FromResult(_isTeamsMode);
        }
    }

    public Task<bool> GetIsTimeAttackModeAsync()
    {
        lock (_isTimeAttackModeMutex)
        {
            return Task.FromResult(_isTimeAttackMode);
        }
    }
}
