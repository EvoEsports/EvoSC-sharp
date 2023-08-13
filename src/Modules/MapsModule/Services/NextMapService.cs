using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.Maps.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.Maps.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class NextMapService : INextMapService
{

    private readonly ILogger<NextMapService> _logger;
    private readonly IMapService _mapService;

    public NextMapService(ILogger<NextMapService> logger, IMapService mapService)
    {
        _logger = logger;
        _mapService = mapService;
    }

    public async Task<IMap> GetNextMapAsync()
    {
        _logger.LogDebug("Getting next map from server");
        var nextMap = await _mapService.GetNextMapAsync();

        if (nextMap == null)
        {
            throw new InvalidOperationException("Failed fetching next map from server.");
        }

        return nextMap;
    }
}
