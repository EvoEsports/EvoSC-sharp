using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Official.MapListModule.Interfaces;

namespace EvoSC.Modules.Official.MapListModule.Controllers;

[Controller]
public class MapListCommandController(IMapListService mapList) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("maplist", "Show a list of available maps.")]
    [CommandAlias("/maps")]
    [CommandAlias("/tracklist")]
    [CommandAlias("/tracks")]
    public Task MapListAsync() => mapList.ShowMapListAsync(Context.Player);
}
