using EvoSC.Common.Interfaces;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.GameModeUiModule.Enums;
using EvoSC.Modules.Official.GameModeUiModule.Interfaces;
using EvoSC.Modules.Official.ScoreboardModule.Config;
using EvoSC.Modules.Official.ScoreboardModule.Interfaces;

namespace EvoSC.Modules.Official.ScoreboardModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class ScoreboardService(
    IManialinkManager manialinks,
    IServerClient server,
    IScoreboardNicknamesService nicknamesService,
    IScoreboardSettings settings,
    IGameModeUiModuleService gameModeUiModuleService
)
    : IScoreboardService
{
    private const string ScoreboardTemplate = "ScoreboardModule.Scoreboard";

    public async Task SendScoreboardAsync()
    {
        await manialinks.SendPersistentManialinkAsync(ScoreboardTemplate, await GetDataAsync());
        await nicknamesService.SendNicknamesManialinkAsync();
    }

    private async Task<dynamic> GetDataAsync()
    {
        var currentNextMaxPlayers = await server.Remote.GetMaxPlayersAsync();
        var maxPlayers = 128;

        if (currentNextMaxPlayers.CurrentValue > 0)
        {
            maxPlayers = currentNextMaxPlayers.CurrentValue;
        }
        else if (currentNextMaxPlayers.NextValue > 0)
        {
            maxPlayers = currentNextMaxPlayers.NextValue;
        }

        return new { settings, MaxPlayers = maxPlayers };
    }

    public Task HideNadeoScoreboardAsync() =>
        gameModeUiModuleService.ApplyComponentSettingsAsync(
            GameModeUiComponents.ScoresTable,
            false,
            0.0,
            0.0,
            1.0
        );

    public Task ShowNadeoScoreboardAsync() =>
        gameModeUiModuleService.ApplyComponentSettingsAsync(
            GameModeUiComponents.ScoresTable,
            true,
            0.0,
            0.0,
            1.0
        );
}
