using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks.Interfaces;

namespace EvoSC.Modules.Official.MapListModule.Controllers;

[Controller]
public class MapListCommandController(IManialinkManager manialinks) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("maplist", "Show a list of available maps.")]
    [CommandAlias("/maps")]
    public Task MapListAsync() =>
        manialinks.SendManialinkAsync(Context.Player, "MapListModule.MapList");
}
