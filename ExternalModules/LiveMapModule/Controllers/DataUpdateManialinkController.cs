using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.snixtho.LiveMapModule.Controllers;

[Controller]
public class DataUpdateManialinkController : ManialinkController
{
    private readonly ILogger<DataUpdateManialinkController> _logger;
    
    public DataUpdateManialinkController(ILogger<DataUpdateManialinkController> logger)
    {
        _logger = logger;
    }
    
    public async Task UpdatePositionAsync(double x, double y, double z)
    {
        _logger.LogDebug($"X: {x}, Y: {y}, Z: {z}");
    }
}
