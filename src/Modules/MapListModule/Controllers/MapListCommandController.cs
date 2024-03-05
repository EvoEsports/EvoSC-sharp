using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Models.Maps;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MapListModule.Models;

namespace EvoSC.Modules.Official.MapListModule.Controllers;

[Controller]
public class MapListCommandController(IManialinkManager manialinks) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("maplist", "Show a list of available maps.")]
    [CommandAlias("/maps")]
    public Task MapListAsync() =>
        manialinks.SendManialinkAsync(Context.Player, "MapListModule.MapList",
            new
            {
                maps = new MapListMapDto[]
                {
                    new() { Map = new Map { Name = "Map 1" } },
                    new() { Map = new Map { Name = "Map 2" } },
                    new() { Map = new Map { Name = "Map 3" } },
                    new() { Map = new Map { Name = "Map 4" } },
                    new() { Map = new Map { Name = "Map 5" } },
                    new() { Map = new Map { Name = "Map 6" } },
                    new() { Map = new Map { Name = "Map 7" } },
                    new() { Map = new Map { Name = "Map 8" } },
                    new() { Map = new Map { Name = "Map 9" } },
                    new() { Map = new Map { Name = "Map 10" } },
                    new() { Map = new Map { Name = "Map 11" } },
                    new() { Map = new Map { Name = "Map 12" } },
                    new() { Map = new Map { Name = "Map 13" } },
                    new() { Map = new Map { Name = "Map 14" } },
                    new() { Map = new Map { Name = "Map 15" } },
                    new() { Map = new Map { Name = "Map 16" } },
                    new() { Map = new Map { Name = "Map 17" } },
                    new() { Map = new Map { Name = "Map 18" } },
                    new() { Map = new Map { Name = "Map 19" } },
                    new() { Map = new Map { Name = "Map 20" } },
                } 
                
            });
}
