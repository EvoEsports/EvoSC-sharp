using EvoSC.Common.Interfaces;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Config;
using EvoSC.Modules.Official.LiveRankingModule.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.LiveRankingModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class LiveRankingService(IManialinkManager manialinkManager, IServerClient server, ILiveRankingSettings settings, ILogger<LiveRankingService> logger) : ILiveRankingService
{
    private const string WidgetTemplate = "LiveRankingModule.LiveRanking";
    
    public async Task Initialize()
    {
        await server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.GetScores");
    }

    public async Task HandleScoresAsync(ScoresEventArgs scores)
    {
        //TODO: resolve account ids to IPlayer
        var mappedScores = scores.Players.Take(settings.MaxWidgetRows);
        
        await manialinkManager.SendPersistentManialinkAsync(WidgetTemplate, new
        {
            isPointsBased = scores.UseTeams, //TODO: detect rounds properly
            isTeamsMode = scores.UseTeams,
            scores = mappedScores,
            settings
        });
    }

    public async Task HideWidgetAsync()
    {
        await manialinkManager.HideManialinkAsync(WidgetTemplate);
    }
}
