using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util.MatchSettings;
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

    private readonly CheckpointsRepository _checkpointsRepository = new();
    private readonly PointsRepartition _pointsRepartition = [];
    private readonly Dictionary<PlayerTeam, string> _teamColors = new();
    private bool _isTimeAttackMode;
    private bool _isTeamsMode;

    public async Task ConsumeCheckpointDataAsync(CheckpointData checkpointData)
    {
        if (_isTimeAttackMode && checkpointData.IsDNF)
        {
            _checkpointsRepository.Remove(checkpointData.Player.AccountId, out var removedCheckpointData);
        }
        else
        {
            _checkpointsRepository[checkpointData.Player.AccountId] = checkpointData;
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

        _checkpointsRepository.Remove(accountId, out var removedCheckpointData);

        await SendRoundRankingWidgetAsync();
    }

    public async Task ClearCheckpointDataAsync()
    {
        _checkpointsRepository.Clear();

        await SendRoundRankingWidgetAsync();
    }

    public async Task SendRoundRankingWidgetAsync()
    {
        CheckpointsRepository cpRepository;
        cpRepository = _checkpointsRepository;

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

        var showWinnerTeam = ShouldShowWinnerTeam(bestCheckpoints);
        var winnerTeamName = "DRAW";
        var winnerTeamColor = theme.Theme.UI_AccentSecondary;
        var winnerTeam = PlayerTeam.Unknown;

        if (showWinnerTeam)
        {
            winnerTeam = GetWinnerTeam(bestCheckpoints);

            if (winnerTeam != PlayerTeam.Unknown)
            {
                winnerTeamColor = _teamColors[winnerTeam];
                winnerTeamName = (await server.Remote.GetTeamInfoAsync((int)winnerTeam + 1)).Name;
            }
        }

        await manialinkManager.SendPersistentManialinkAsync(WidgetTemplate,
            new
            {
                settings,
                showWinnerTeam,
                winnerTeam,
                winnerTeamName,
                winnerTeamColor,
                bestCheckpoints = bestCheckpoints.Take(settings.MaxRows),
            });
    }

    public Task HideRoundRankingWidgetAsync() =>
        manialinkManager.HideManialinkAsync(WidgetTemplate);

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

    public async Task DetectIsTeamsModeAsync()
    {
        var currentMode = await matchSettingsService.GetCurrentModeAsync();
        _isTeamsMode = currentMode == DefaultModeScriptName.Teams;
    }

    public async Task FetchAndCacheTeamInfoAsync()
    {
        _teamColors[PlayerTeam.Unknown] = theme.Theme.UI_AccentSecondary;
        _teamColors[PlayerTeam.Team1] = (await server.Remote.GetTeamInfoAsync((int)PlayerTeam.Team1 + 1)).RGB;
        _teamColors[PlayerTeam.Team2] = (await server.Remote.GetTeamInfoAsync((int)PlayerTeam.Team2 + 1)).RGB;
    }

    public bool ShouldShowWinnerTeam(List<CheckpointData> checkpoints) =>
        _isTeamsMode && checkpoints.Any(checkpoint => checkpoint.IsFinish);

    public void SetGainedPointsOnResult(List<CheckpointData> checkpoints)
    {
        var rank = 1;
        foreach (var cpData in checkpoints.Where(checkpoint => checkpoint.IsFinish))
        {
            cpData.GainedPoints = _pointsRepartition.GetGainedPoints(rank++);
        }
    }

    public PlayerTeam GetWinnerTeam(List<CheckpointData> checkpoints)
    {
        var gainedPointsPerTeam = new Dictionary<PlayerTeam, int>
        {
            { PlayerTeam.Unknown, 0 }, { PlayerTeam.Team1, 0 }, { PlayerTeam.Team2, 0 }
        };

        foreach (var cpData in checkpoints)
        {
            gainedPointsPerTeam[cpData.Player.Team] += cpData.GainedPoints;
        }

        if (gainedPointsPerTeam[PlayerTeam.Team1] == gainedPointsPerTeam[PlayerTeam.Team2])
        {
            return PlayerTeam.Unknown;
        }

        return gainedPointsPerTeam.Where(tp => tp.Value > 0)
            .OrderByDescending(tp => tp.Value)
            .Select(tp => tp.Key)
            .FirstOrDefault(PlayerTeam.Unknown);
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
        foreach (var cpData in checkpoints.Where(checkpoint => checkpoint.GainedPoints > 0))
        {
            cpData.AccentColor = _isTeamsMode
                ? _teamColors[cpData.Player.Team]
                : (string)theme.Theme.UI_AccentPrimary;
        }
    }
}
