using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.NextMapModule.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.NextMapModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class NextMapService(ILogger<NextMapService> logger, IMapService mapService) : INextMapService
{
    public async Task<IMap> GetNextMapAsync()
    {
        logger.LogDebug("Getting next map from server");
        var nextMap = await mapService.GetNextMapAsync();

        if (nextMap == null)
        {
            throw new InvalidOperationException("Failed fetching next map from server.");
        }

        return nextMap;
    }
}
