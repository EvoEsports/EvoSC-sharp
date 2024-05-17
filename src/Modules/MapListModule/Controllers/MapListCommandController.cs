using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MapListModule.Interfaces;

namespace EvoSC.Modules.Official.MapListModule.Controllers;

[Controller]
public class MapListCommandController(IManialinkManager manialinks, IMapListService mapList) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("maplist", "Show a list of available maps.")]
    [CommandAlias("/maps")]
    [CommandAlias("/tracklist")]
    [CommandAlias("/tracks")]
    public async Task MapListAsync()
    {
        var maps = await mapList.GetCurrentMapsForPlayerAsync(Context.Player);
        
        await manialinks.SendManialinkAsync(Context.Player, "MapListModule.MapList", new
        {
            maps = maps
        });
    }
}
