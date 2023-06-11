using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.OpenPlanetControl.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.OpenPlanetControl.Services;

public class OpenPlanetControlService : IOpenPlanetControlService
{
    private readonly ILogger<OpenPlanetControlService> _logger;
    private readonly IManialinkManager _manialinkManager;

    public OpenPlanetControlService(ILogger<OpenPlanetControlService> logger, IManialinkManager manialinkManager)
    {
        _logger = logger;
        _manialinkManager = manialinkManager;
    }

    public async Task onEnable()
    {
        _logger.LogInformation("OpenPlanetControl enabled.");
        await _manialinkManager.SendManialinkAsync("OpenPlanetControl.OpenPlanetControlWidget",
            new { data = "Hello from OpenPlanetControl!" });
    }

    public async Task onDisable()
    {
        await _manialinkManager.HideManialinkAsync("OpenPlanetControl.OpenPlanetControlWidget");
        _logger.LogInformation("OpenPlanetControl disabled.");
    }
}
