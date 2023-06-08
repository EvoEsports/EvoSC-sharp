using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.ModuleManagerModule.Services;

[Service]
public class ModuleUpdateChecker : IBackgroundService
{
    private readonly ILogger<ModuleUpdateChecker> _logger;
    
    public ModuleUpdateChecker(ILogger<ModuleUpdateChecker> logger)
    {
        _logger = logger;
    }
    
    public Task StartAsync()
    {
        _logger.LogInformation("background service started!");
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        _logger.LogInformation("background service stopped!");
        return Task.CompletedTask;
    }
}
