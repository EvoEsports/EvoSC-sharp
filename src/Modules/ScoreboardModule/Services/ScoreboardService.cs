using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.ScoreboardModule.Config;
using EvoSC.Modules.Official.ScoreboardModule.Interfaces;

namespace EvoSC.Modules.Official.ScoreboardModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class ScoreboardService(
    IManialinkManager manialinks,
    IServerClient server,
    IScoreboardNicknamesService nicknamesService,
    IThemeManager themes,
    IScoreboardSettings settings
)
    : IScoreboardService
{
    public static readonly string ScoreboardTemplate = "ScoreboardModule.Scoreboard";
    
    public async Task SendScoreboardAsync()
    {
        await manialinks.SendPersistentManialinkAsync(ScoreboardTemplate, await GetDataAsync());
        await nicknamesService.SendNicknamesManialinkAsync();
    }

    private Task<dynamic> GetDataAsync()
    {
        return Task.FromResult<dynamic>(new
        {
            settings,
            MaxPlayers = 64, //TODO: make dynamic
            PositionColors = new Dictionary<int, string>
            {
                { 1, themes.Theme.Gold }, { 2, themes.Theme.Silver }, { 3, themes.Theme.Bronze }
            },
        });
    }

    public async Task HideNadeoScoreboardAsync()
    {
        var hudSettings = new List<string>
        {
            @"{
    ""uimodules"": [
        {
            ""id"": ""Race_ScoresTable"",
            ""position"": [-50,0],
            ""scale"": 1,
            ""visible"": false,
            ""visible_update"": true
        }
    ]
}"
        };

        await server.Remote.TriggerModeScriptEventArrayAsync("Common.UIModules.SetProperties", hudSettings.ToArray());
    }

    public async Task ShowNadeoScoreboardAsync()
    {
        var hudSettings = new List<string>
        {
            @"{
    ""uimodules"": [
        {
            ""id"": ""Race_ScoresTable"",
            ""position"": [-50,0],
            ""scale"": 1,
            ""visible"": true,
            ""visible_update"": true
        }
    ]
}"
        };

        await server.Remote.TriggerModeScriptEventArrayAsync("Common.UIModules.SetProperties", hudSettings.ToArray());
    }
}
