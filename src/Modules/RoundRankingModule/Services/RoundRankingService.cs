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
    public const string WidgetTemplate = "RoundRankingModule.RoundRanking";

    private readonly object _checkpointsRepositoryMutex = new();
    private readonly CheckpointsRepository _checkpointsRepository = new();
    private readonly List<int> _pointsRepartition = [];
    private readonly Dictionary<PlayerTeam, string> _teamColors = new();
    private bool _isTimeAttackMode;
    private bool _isTeamsMode;

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

        var bestCheckpoints = SortCheckpointData(cpRepository);

        if (bestCheckpoints.Count > 0)
        {
            if (!_isTimeAttackMode)
            {
                SetGainedPointsOnResult(bestCheckpoints);
            }

            SetAccentColorsOnResult(bestCheckpoints);

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
        lock (_checkpointsRepositoryMutex)
        {
            if (_checkpointsRepository.ContainsKey(accountId))
            {
                return true;
            }

            return _checkpointsRepository.Count < settings.MaxRows;
        }
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

    public Task SetIsTimeAttackModeAsync(bool isTimeAttackMode)
    {
        _isTimeAttackMode = isTimeAttackMode;

        return Task.CompletedTask;
    }

    public async Task DetectModeAsync()
    {
        var currentMode = await matchSettingsService.GetCurrentModeAsync();
        _isTimeAttackMode = currentMode is DefaultModeScriptName.TimeAttack;
        _isTeamsMode = currentMode is DefaultModeScriptName.Teams or DefaultModeScriptName.TmwtTeams;
    }

    public async Task FetchAndCacheTeamInfoAsync()
    {
        _teamColors[PlayerTeam.Team1] = (await server.Remote.GetTeamInfoAsync((int)PlayerTeam.Team1 + 1)).RGB;
        _teamColors[PlayerTeam.Team2] = (await server.Remote.GetTeamInfoAsync((int)PlayerTeam.Team2 + 1)).RGB;
    }

    public void SetGainedPointsOnResult(List<CheckpointData> checkpoints)
    {
        int rank = 1;
        foreach (var checkpoint in checkpoints.Where(checkpoint => checkpoint.IsFinish))
        {
            checkpoint.GainedPoints = GetGainedPointsForRank(rank++);
        }
    }

    public string? GetTeamAccentColor(PlayerTeam winnerTeam, PlayerTeam playerTeam)
    {
        var useTeamColor = winnerTeam == playerTeam || winnerTeam == PlayerTeam.Unknown;

        return useTeamColor ? _teamColors[playerTeam] : null;
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

    public int GetGainedPointsForRank(int rank)
    {
        return rank <= _pointsRepartition.Count ? _pointsRepartition[rank - 1] : _pointsRepartition.LastOrDefault(0);
    }

    public void SetAccentColorsOnResult(List<CheckpointData> checkpoints)
    {
        var winnerTeam = GetWinnerTeam(checkpoints);

        foreach (var checkpoint in checkpoints.Where(checkpoint => checkpoint.GainedPoints > 0))
        {
            checkpoint.AccentColor = _isTeamsMode
                ? GetTeamAccentColor(winnerTeam, checkpoint.Player.Team)
                : (string)theme.Theme.UI_AccentPrimary;
        }
    }

    public List<CheckpointData> SortCheckpointData(CheckpointsRepository checkpoints)
    {
        return checkpoints.Values
            .OrderBy(cpData => cpData.IsDNF)
            .ThenByDescending(cpData => cpData.CheckpointId)
            .ThenBy(cpData => cpData.Time.TotalMilliseconds)
            .ToList();
    }
}
