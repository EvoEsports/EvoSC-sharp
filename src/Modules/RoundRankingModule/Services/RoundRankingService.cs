using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.RoundRankingModule.Config;
using EvoSC.Modules.Official.RoundRankingModule.Interfaces;
using EvoSC.Modules.Official.RoundRankingModule.Models;

namespace EvoSC.Modules.Official.RoundRankingModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class RoundRankingService(
    IRoundRankingSettings settings,
    IManialinkManager manialinkManager
) : IRoundRankingService
{
    public const string WidgetTemplate = "RoundRankingModule.RoundRanking";

    private readonly object _checkpointsRepositoryMutex = new();
    private readonly CheckpointsRepository _checkpointsRepository = new();

    public Task AddCheckpointDataAsync(IOnlinePlayer player, int checkpointIndex, int checkpointTime, bool isFinish)
    {
        lock (_checkpointsRepositoryMutex)
        {
            _checkpointsRepository.AddAndSort(new CheckpointData
            {
                Player = player,
                CheckpointId = checkpointIndex,
                Time = RaceTime.FromMilliseconds(checkpointTime),
                IsFinish = isFinish
            });
        }

        return Task.CompletedTask;
    }

    public Task RemoveCheckpointDataAsync(IOnlinePlayer player, int checkpointIndex)
    {
        if (checkpointIndex < 0)
        {
            return Task.CompletedTask;
        }

        lock (_checkpointsRepositoryMutex)
        {
            if (!_checkpointsRepository.TryGetValue(checkpointIndex, out var checkpointGroup))
            {
                return Task.CompletedTask;
            }

            var playersEntry = checkpointGroup
                .FirstOrDefault(cpData => cpData.Player.AccountId == player.AccountId);

            if (playersEntry != null)
            {
                _checkpointsRepository[checkpointIndex].Remove(playersEntry);
            }
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

        await manialinkManager.SendPersistentManialinkAsync(WidgetTemplate, new { settings, bestCheckpoints });
    }

    public Task HideRoundRankingWidgetAsync() =>
        manialinkManager.HideManialinkAsync(WidgetTemplate);
}
