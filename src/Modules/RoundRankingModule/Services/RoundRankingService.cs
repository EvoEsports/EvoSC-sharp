using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
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
    IMatchSettingsService matchSettingsService,
    IThemeManager theme,
    IServerClient server
) : IRoundRankingService
{
    private const string WidgetTemplate = "RoundRankingModule.RoundRanking";

    private readonly object _checkpointsRepositoryMutex = new();
    private readonly CheckpointsRepository _checkpointsRepository = new();
    private readonly PointsRepartition _pointsRepartition = new();
    private readonly Dictionary<PlayerTeam, string> _teamColors = new();
    private bool _isTimeAttackMode;
    private bool _isTeamsMode = false;

    public async Task ConsumeCheckpointDataAsync(CheckpointData checkpointData)
    {
        lock (_checkpointsRepositoryMutex)
        {
            if (_isTimeAttackMode && checkpointData.IsDNF)
            {
                _checkpointsRepository.Remove(checkpointData.Player.AccountId);
            }
            else
            {
                _checkpointsRepository[checkpointData.Player.AccountId] = checkpointData;
            }
        }

        await SendRoundRankingWidgetAsync();
    }

    public async Task RemovePlayerCheckpointDataAsync(string accountId)
    {
        if (!_isTimeAttackMode)
        {
            //In time attack mode the entries are cleared by new round event.
            //Prevents flood of manialinks.
            return;
        }

        lock (_checkpointsRepositoryMutex)
        {
            _checkpointsRepository.Remove(accountId);
        }

        await SendRoundRankingWidgetAsync();
    }

    public async Task ClearCheckpointDataAsync()
    {
        lock (_checkpointsRepositoryMutex)
        {
            _checkpointsRepository.Clear();
        }

        await SendRoundRankingWidgetAsync();
    }

    public async Task SendRoundRankingWidgetAsync()
    {
        CheckpointsRepository cpRepository;
        lock (_checkpointsRepositoryMutex)
        {
            cpRepository = _checkpointsRepository;
        }

        var bestCheckpoints = cpRepository.GetSortedData();

        if (bestCheckpoints.Count > 0)
        {
            bestCheckpoints = bestCheckpoints.Take(settings.MaxRows).ToList();
            
            if (settings.DisplayGainedPoints && !_isTimeAttackMode)
            {
                SetGainedPointsOnResult(bestCheckpoints);
            }

            SetGainedPointsBackgroundColorsOnResult(bestCheckpoints);

            if (settings.DisplayTimeDifference)
            {
                CalculateAndSetTimeDifferenceOnResult(bestCheckpoints);
            }
        }

        await manialinkManager.SendPersistentManialinkAsync(WidgetTemplate,
            new { settings, bestCheckpoints = bestCheckpoints.Take(settings.MaxRows) });
    }

    public Task HideRoundRankingWidgetAsync() =>
        manialinkManager.HideManialinkAsync(WidgetTemplate);

    public bool ShouldCollectCheckpointData(string accountId)
    {
        return true;
    }

    public async Task LoadPointsRepartitionFromSettingsAsync()
    {
        var modeScriptSettings = await matchSettingsService.GetCurrentScriptSettingsAsync();
        var pointsRepartitionString =
            (string?)modeScriptSettings?.GetValueOrDefault(PointsRepartition.ModeScriptSetting);

        if (pointsRepartitionString != null && pointsRepartitionString.Trim().Length > 0)
        {
            _pointsRepartition.Update(pointsRepartitionString);
        }
    }

    public Task SetIsTimeAttackModeAsync(bool isTimeAttackMode)
    {
        _isTimeAttackMode = isTimeAttackMode;

        return Task.CompletedTask;
    }

    public async Task FetchAndCacheTeamInfoAsync()
    {
        _teamColors[PlayerTeam.Team1] = (await server.Remote.GetTeamInfoAsync((int)PlayerTeam.Team1 + 1)).RGB;
        _teamColors[PlayerTeam.Team2] = (await server.Remote.GetTeamInfoAsync((int)PlayerTeam.Team2 + 1)).RGB;
    }

    public void SetGainedPointsOnResult(List<CheckpointData> checkpoints)
    {
        var rank = 1;
        foreach (var cpData in checkpoints.Where(checkpoint => checkpoint.IsFinish))
        {
            cpData.GainedPoints = _pointsRepartition.GetGainedPoints(rank++);
        }
    }

    public string? GetTeamAccentColor(PlayerTeam winnerTeam, PlayerTeam playerTeam)
    {
        return winnerTeam == playerTeam || winnerTeam == PlayerTeam.Unknown
            ? _teamColors[playerTeam]
            : null;
    }

    public PlayerTeam GetWinnerTeam(List<CheckpointData> checkpoints)
    {
        var teamPoints = new Dictionary<PlayerTeam, int>
        {
            { PlayerTeam.Unknown, 0 }, { PlayerTeam.Team1, 0 }, { PlayerTeam.Team2, 0 }
        };

        foreach (var cpData in checkpoints)
        {
            teamPoints[cpData.Player.Team] += cpData.GainedPoints;
        }

        return teamPoints.OrderByDescending(tp => tp.Value)
            .First()
            .Key;
    }

    public void CalculateAndSetTimeDifferenceOnResult(List<CheckpointData> checkpoints)
    {
        if (checkpoints.Count <= 1)
        {
            return;
        }

        var firstEntry = checkpoints.FirstOrDefault();
        if (firstEntry == null)
        {
            return;
        }

        firstEntry.TimeDifference = null;
        foreach (var cpData in checkpoints[1..])
        {
            cpData.TimeDifference = cpData.GetTimeDifferenceAbsolute(firstEntry);
        }
    }

    public void SetGainedPointsBackgroundColorsOnResult(List<CheckpointData> checkpoints)
    {
        var winnerTeam = _isTeamsMode ? GetWinnerTeam(checkpoints) : PlayerTeam.Unknown;

        foreach (var cpData in checkpoints.Where(checkpoint => checkpoint.GainedPoints > 0))
        {
            cpData.AccentColor = _isTeamsMode
                ? GetTeamAccentColor(winnerTeam, cpData.Player.Team)
                : (string)theme.Theme.UI_AccentPrimary;
        }
    }
}
