using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.RoundRankingModule.Config;
using EvoSC.Modules.Official.RoundRankingModule.Interfaces;
using EvoSC.Modules.Official.RoundRankingModule.Models;
using EvoSC.Modules.Official.RoundRankingModule.Utils;
using LinqToDB.Common;

namespace EvoSC.Modules.Official.RoundRankingModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class RoundRankingService(
    IRoundRankingStateService stateService,
    IRoundRankingSettings settings,
    IManialinkManager manialinkManager,
    IPlayerManagerService playerManagerService,
    IMatchSettingsService matchSettingsService,
    IThemeManager theme,
    IServerClient server
) : IRoundRankingService
{
    private const string WidgetTemplate = "RoundRankingModule.RoundRanking";

    public async Task ConsumeCheckpointAsync(string accountId, int checkpointId, int time, bool isFinish, bool isDnf)
    {
        await stateService.UpdateRepositoryEntryAsync(accountId,
            new CheckpointData
            {
                Player = await playerManagerService.GetOnlinePlayerAsync(accountId),
                CheckpointId = checkpointId,
                Time = RaceTime.FromMilliseconds(time),
                IsFinish = isFinish,
                IsDNF = isDnf
            });

        await SendRoundRankingWidgetAsync();
    }

    public async Task ConsumeDnfAsync(string accountId)
    {
        var isTimeAttackMode = await stateService.GetIsTimeAttackModeAsync();

        if (!isTimeAttackMode)
        {
            await ConsumeCheckpointAsync(accountId, -1, 0, false, true);
            return;
        }

        await stateService.RemoveRepositoryEntryAsync(accountId);
        await SendRoundRankingWidgetAsync();
    }

    public async Task RemovePlayerCheckpointDataAsync(string accountId)
    {
        var isTimeAttackMode = await stateService.GetIsTimeAttackModeAsync();

        if (!isTimeAttackMode)
        {
            //In time attack mode the entries are cleared by new round event.
            //Prevents flood of manialinks.
            return;
        }

        await stateService.RemoveRepositoryEntryAsync(accountId);
        await SendRoundRankingWidgetAsync();
    }

    public async Task<List<CheckpointData>> GetSortedCheckpointsAsync()
    {
        var cpRepository = await stateService.GetRepositoryAsync();

        return cpRepository.IsNullOrEmpty() ? [] : cpRepository.GetSortedData();
    }

    public async Task ClearCheckpointDataAsync()
    {
        await stateService.ClearRepositoryAsync();
        await SendRoundRankingWidgetAsync();
    }

    public async Task SendRoundRankingWidgetAsync()
    {
        var bestCheckpoints = await GetSortedCheckpointsAsync();
        var isTeamsMode = await stateService.GetIsTeamsModeAsync();
        var teamColors = await stateService.GetTeamColorsAsync();

        if (bestCheckpoints.Count > 0)
        {
            var isTimeAttackMode = await stateService.GetIsTimeAttackModeAsync();
            bestCheckpoints = bestCheckpoints.Take(settings.MaxRows).ToList();

            if (settings.DisplayGainedPoints && !isTimeAttackMode)
            {
                RoundRankingUtils.SetGainedPointsOnResult(
                    bestCheckpoints,
                    await stateService.GetPointsRepartitionAsync(),
                    (string)theme.Theme.UI_AccentPrimary
                );
            }

            if (isTeamsMode)
            {
                RoundRankingUtils.ApplyTeamColorsAsAccentColors(bestCheckpoints, teamColors);
            }

            if (settings.DisplayTimeDifference)
            {
                RoundRankingUtils.CalculateAndSetTimeDifferenceOnResult(bestCheckpoints);
            }
        }

        var showWinnerTeam = isTeamsMode && RoundRankingUtils.HasPlayerInFinish(bestCheckpoints);
        var winnerTeamName = "DRAW";
        var winnerTeamColor = theme.Theme?.UI_AccentSecondary ?? "000000";
        var winnerTeam = PlayerTeam.Unknown;

        if (showWinnerTeam)
        {
            winnerTeam = RoundRankingUtils.GetWinnerTeam(bestCheckpoints);

            if (winnerTeam != PlayerTeam.Unknown)
            {
                winnerTeamColor = teamColors[winnerTeam];
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

    public async Task LoadPointsRepartitionFromSettingsAsync()
    {
        var modeScriptSettings = await matchSettingsService.GetCurrentScriptSettingsAsync();
        var pointsRepartitionString =
            (string?)modeScriptSettings?.GetValueOrDefault(PointsRepartition.ModeScriptSetting);

        if (pointsRepartitionString == null || pointsRepartitionString.Trim().Length == 0)
        {
            return;
        }

        await stateService.UpdatePointsRepartitionAsync(pointsRepartitionString);
    }

    public Task SetIsTimeAttackModeAsync(bool isTimeAttackMode)
        => stateService.SetIsTimeAttackModeAsync(isTimeAttackMode);

    public async Task DetectAndSetIsTeamsModeAsync()
    {
        var currentMode = await matchSettingsService.GetCurrentModeAsync();
        await stateService.SetIsTeamsModeAsync(currentMode == DefaultModeScriptName.Teams);
    }

    public async Task FetchAndCacheTeamInfoAsync()
    {
        var team1Info = await server.Remote.GetTeamInfoAsync((int)PlayerTeam.Team1 + 1);
        var team2Info = await server.Remote.GetTeamInfoAsync((int)PlayerTeam.Team2 + 1);

        await stateService.SetTeamColorsAsync(team1Info.RGB, team2Info.RGB, theme.Theme.UI_AccentSecondary);
    }

    public Task HideRoundRankingWidgetAsync() =>
        manialinkManager.HideManialinkAsync(WidgetTemplate);
}
