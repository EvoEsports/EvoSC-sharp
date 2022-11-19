using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Modules.Official.Maps.Interfaces;

namespace EvoSC.Modules.Official.Maps.Controllers;

[Controller]
public class MapsController : EvoScController<PlayerInteractionContext>
{
    private IMxMapService _mxMapService;

    public MapsController(IMxMapService mxMapService)
    {
        _mxMapService = mxMapService;
    }

    [ChatCommand("add", "Adds a map to the server")]
    public async Task AddMap(int mapId)
    {
        await _mxMapService.FindAndDownloadMap(mapId, null);
    }
}
