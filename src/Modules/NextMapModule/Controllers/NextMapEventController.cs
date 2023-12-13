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
public class NextMapEventController(INextMapService nextMapService, IManialinkManager manialinkManager)
    : EvoScController<IEventControllerContext>
{
    private const string Template = "NextMapModule.NextMap";

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public async Task ShowNextMapOnPodiumStartAsync(object sender, PodiumEventArgs args)
    {
        var nextMap = await nextMapService.GetNextMapAsync();
        await manialinkManager.SendManialinkAsync(Template,
            new
            {
                mapName = nextMap.Name, 
                author = nextMap.Author?.NickName
            });
    }

    [Subscribe(ModeScriptEvent.PodiumEnd)]
    public async Task HideNextMapOnPodiumEndAsync(object sender, PodiumEventArgs args)
    {
        await manialinkManager.HideManialinkAsync(Template);
    }

    [Subscribe(ModeScriptEvent.StartMapStart)]
    public async Task HideNextMapOnMapStartAsync(object sender, MapEventArgs args)
    {
        await manialinkManager.HideManialinkAsync(Template);
    }

    [Subscribe(ModeScriptEvent.EndMapEnd)]
    public async Task HideNextMapOnMapEndAsync(object sender, MapEventArgs args)
    {
        await manialinkManager.HideManialinkAsync(Template);
    }
}
