using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.NextMapModule.Config;
using EvoSC.Modules.Official.NextMapModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.NextMapModule.Controllers;

[Controller]
public class NextMapEventController(
    INextMapService nextMapService,
    IManialinkManager manialinkManager,
    INextMapSettings settings) : EvoScController<IEventControllerContext>
{
    private const string Template = "NextMapModule.NextMap";

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public async Task ShowNextMapOnPodiumStartAsync(object sender, PodiumEventArgs args)
    {
        var nextMap = await nextMapService.GetNextMapAsync();
        await manialinkManager.SendManialinkAsync(Template,
            new { mapName = nextMap.Name, author = nextMap.Author?.NickName, settings });
    }

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public async Task HideNextMapOnMapStartAsync(object sender, MapGbxEventArgs args)
    {
        await manialinkManager.HideManialinkAsync(Template);
    }
}
