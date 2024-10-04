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

        await DisplayRoundRankingWidgetAsync();
    }

    public Task RemovePlayerCheckpointDataAsync(IOnlinePlayer player) =>
        RemovePlayerCheckpointDataAsync(player.AccountId);

    public async Task RemovePlayerCheckpointDataAsync(string accountId)
    {
        if (!_isTimeAttackMode)
        {
            //In time attack mode the entries are cleared by new round event, prevents flood of manialinks.
            return;
        }

        lock (_checkpointsRepositoryMutex)
        {
            _checkpointsRepository.Remove(accountId);
        }

        await DisplayRoundRankingWidgetAsync();
    }

    public async Task ClearCheckpointDataAsync()
    {
        lock (_checkpointsRepositoryMutex)
        {
            _checkpointsRepository.Clear();
        }

        await DisplayRoundRankingWidgetAsync();
    }

    public async Task DisplayRoundRankingWidgetAsync()
    {
        List<CheckpointData> bestCheckpoints;

        lock (_checkpointsRepositoryMutex)
        {
            bestCheckpoints = _checkpointsRepository.GetBestTimes(settings.MaxRows);
        }

        if (!_isTimeAttackMode)
        {
            int rank = 1;
            foreach (var checkpoint in bestCheckpoints.Where(checkpoint => checkpoint.IsFinish))
            {
                checkpoint.GainedPoints = GetGainedPoints(rank++);
            }
        }

        foreach (var checkpoint in bestCheckpoints.Where(checkpoint => checkpoint.GainedPoints > 0))
        {
            //TODO: get winning team
            checkpoint.AccentColor = _isTeamsMode
                ? GetTeamAccentColor(checkpoint.Player.Team)
                : (string)theme.Theme.UI_AccentPrimary;
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

    public string? GetTeamAccentColor(PlayerTeam playerTeam)
    {
        var winnerTeam = GetWinnerTeam();
        var useTeamColor = winnerTeam == playerTeam || winnerTeam == PlayerTeam.Unknown;

        return useTeamColor ? _teamColors[playerTeam] : null;
    }

    public PlayerTeam GetWinnerTeam()
    {
        return PlayerTeam.Unknown; //TODO: calculate & return winner team
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
}
