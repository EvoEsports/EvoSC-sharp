using EvoSC.Common.Config.Models;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.NextMapModule.Interfaces;

namespace EvoSC.Modules.Official.NextMapModule.Controllers;

[Controller]
public class NextMapEventController : EvoScController<IEventControllerContext>
{
    private const string Template = "NextMapModule.NextMap";
    
    private readonly INextMapService _nextMapService;
    private readonly IManialinkManager _manialinkManager;
    private readonly IEvoScBaseConfig _config;

    public NextMapEventController(INextMapService nextMapService, IManialinkManager manialinkManager, IEvoScBaseConfig config)
    {
        _nextMapService = nextMapService;
        _manialinkManager = manialinkManager;
        _config = config;
    }

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public async Task ShowNextMapOnPodiumStartAsync(object sender, PodiumEventArgs args)
    {
        var nextMap = await _nextMapService.GetNextMapAsync();
        await _manialinkManager.SendManialinkAsync(Template,
            new
            {
                mapName = nextMap.Name, 
                author = nextMap.Author?.NickName,
                headerColor = _config.Theme.UI.HeaderBackgroundColor,
                primaryColor = _config.Theme.UI.PrimaryColor,
                logoUrl = _config.Theme.UI.LogoWhiteUrl,
                playerRowBackgroundColor = _config.Theme.UI.PlayerRowBackgroundColor
            });
    }

    [Subscribe(ModeScriptEvent.PodiumEnd)]
    public async Task HideNextMapOnPodiumEndAsync(object sender, PodiumEventArgs args)
    {
        await _manialinkManager.HideManialinkAsync(Template);
    }

    [Subscribe(ModeScriptEvent.StartMapStart)]
    public async Task HideNextMapOnMapStart(object sender, MapEventArgs args)
    {
        await _manialinkManager.HideManialinkAsync(Template);
    }

    [Subscribe(ModeScriptEvent.EndMapEnd)]
    public async Task HideNextMapOnMapEnd(object sender, MapEventArgs args)
    {
        await _manialinkManager.HideManialinkAsync(Template);
    }
}
