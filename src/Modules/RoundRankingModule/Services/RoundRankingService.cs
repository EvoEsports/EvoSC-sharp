using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.RoundRankingModule.Config;
using EvoSC.Modules.Official.RoundRankingModule.Interfaces;
using EvoSC.Modules.Official.RoundRankingModule.Models;

namespace EvoSC.Modules.Official.RoundRankingModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class RoundRankingService(
    IRoundRankingSettings settings,
    IManialinkManager manialinkManager,
    IServerClient server
) : IRoundRankingService
{
    public const string WidgetTemplate = "RoundRankingModule.RoundRanking";

    private readonly object _checkpointsRepositoryMutex = new();
    private readonly CheckpointsRepository _checkpointsRepository = new();
    private readonly List<int> _pointsRepartition = [];

    public Task AddCheckpointDataAsync(CheckpointData checkpointData)
    {
        lock (_checkpointsRepositoryMutex)
        {
            _checkpointsRepository[checkpointData.Player.AccountId] = checkpointData;
        }

        return Task.CompletedTask;
    }

    public Task RemovePlayerCheckpointDataAsync(IOnlinePlayer player)
    {
        lock (_checkpointsRepositoryMutex)
        {
            _checkpointsRepository.Remove(player.AccountId);
        }

        return Task.CompletedTask;
    }

    public Task ClearCheckpointDataAsync()
    {
        lock (_checkpointsRepositoryMutex)
        {
            _checkpointsRepository.Clear();
        }

        return Task.CompletedTask;
    }

    public async Task DisplayRoundRankingWidgetAsync()
    {
        List<CheckpointData> bestCheckpoints;

        lock (_checkpointsRepositoryMutex)
        {
            bestCheckpoints = _checkpointsRepository.GetBestTimes(settings.MaxRows);
        }

        int rank = 1;
        foreach (var checkpoint in bestCheckpoints.Where(checkpoint => checkpoint.IsFinish))
        {
            checkpoint.GainedPoints = GetGainedPoints(rank++);
        }

        await manialinkManager.SendPersistentManialinkAsync(WidgetTemplate, new { settings, bestCheckpoints });
    }

    public Task HideRoundRankingWidgetAsync() =>
        manialinkManager.HideManialinkAsync(WidgetTemplate);

    public bool ShouldCollectCheckpointData(string playerAccountId)
    {
        lock (_checkpointsRepositoryMutex)
        {
            if (_checkpointsRepository.ContainsKey(playerAccountId))
            {
                return true;
            }

            return _checkpointsRepository.Count < settings.MaxRows;
        }
    }

    public int GetGainedPoints(int rank)
    {
        return rank <= _pointsRepartition.Count ? _pointsRepartition[rank - 1] : _pointsRepartition.LastOrDefault(0);
    }

    public async Task UpdatePointsRepartitionAsync()
    {
        var modeScriptSettings = await server.Remote.GetModeScriptSettingsAsync();
        var pointsRepartitionString =
            (string)modeScriptSettings.GetValueOrDefault("S_PointsRepartition", "10,6,4,3,2,1");
        var pointsRepartition = pointsRepartitionString.Split(',').Select(int.Parse).ToList();

        _pointsRepartition.Clear();
        _pointsRepartition.AddRange(pointsRepartition);
    }
}
