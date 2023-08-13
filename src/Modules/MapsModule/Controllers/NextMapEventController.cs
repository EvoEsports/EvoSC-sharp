using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.Maps.Interfaces;

namespace EvoSC.Modules.Official.Maps.Controllers;

[Controller]
public class NextMapEventController : EvoScController<IEventControllerContext>
{
    private const string Template = "Maps.NextMap";
    
    private readonly INextMapService _nextMapService;
    private readonly IManialinkManager _manialinkManager;
    
    public NextMapEventController(INextMapService nextMapService, IManialinkManager manialinkManager)
    {
        _nextMapService = nextMapService;
        _manialinkManager = manialinkManager;
    }

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public async Task ShowNextMapOnPodiumStart(object sender, PodiumEventArgs args)
    {
        var nextMap = await _nextMapService.GetNextMapAsync();
        await _manialinkManager.SendManialinkAsync(Template,
            new { mapName = nextMap.Name, author = nextMap.Author?.NickName });
    }

    [Subscribe(ModeScriptEvent.PodiumEnd)]
    public async Task HideNextMapOnPodiumEnd(object sender, PodiumEventArgs args)
    {
        await _manialinkManager.HideManialinkAsync(Template);
    }
}
