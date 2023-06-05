using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.EvoSCTemplateModule.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.EvoSCTemplateModule.Services;

public class EvoSCTemplateService : IEvoSCTemplateService
{
    private readonly ILogger<EvoSCTemplateService> _logger;
    private readonly IManialinkManager _manialinkManager;
    
    public EvoSCTemplateService(ILogger<EvoSCTemplateService> logger, IManialinkManager manialinkManager)
    {
        _logger = logger;
        _manialinkManager = manialinkManager;
    }

    public async Task onEnable()
    {
        _logger.LogInformation("EvoSCTemplateModule enabled.");
        await _manialinkManager.SendManialinkAsync("EvoSCTemplateModule.EvoSCTemplateWidget",
            new
            {
                data = "Hello from EvoSCTemplateModule!"
            });
    }

    public async Task onDisable()
    {
        await _manialinkManager.HideManialinkAsync("EvoSCTemplateModule.EvoSCTemplateWidget");
        _logger.LogInformation("EvoSCTemplateModule disabled.");
    }
}
