using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Maps;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MapListModule.Models;

namespace EvoSC.Modules.Official.MapListModule.Controllers;

[Controller]
public class MapListCommandController(IManialinkManager manialinks, IMatchSettingsService matchSettings, IServerClient server) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("maplist", "Show a list of available maps.")]
    [CommandAlias("/maps")]
    public async Task MapListAsync()
    {
        var mapsDir = await server.GetMapsDirectoryAsync();
        var maps = await matchSettings.GetCurrentMapListAsync();

        await manialinks.SendManialinkAsync(Context.Player, "MapListModule.MapList", new
        {
            maps = maps.Select(map => new MapListMapDto
            {
                Map = map
            }).ToArray() 
        });
    }
    
    /*  =>
        manialinks.SendManialinkAsync(Context.Player, "MapListModule.MapList",
            new
            {
                maps = Enumerable
                    .Range(0, 20)
                    .Select(i => new MapListMapDto
                    {
                        Map = new Map
                        {
                            Name = $"Map {i}",
                            Author = new Player
                            {
                                NickName = $"Author {i}"
                            }
                        }
                    }
                ).ToArray()
            }); */
}
