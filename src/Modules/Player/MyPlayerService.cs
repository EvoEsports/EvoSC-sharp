using EvoSC.Common.Interfaces;
using EvoSC.Modules.Attributes;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace EvoSC.Modules.Official.Player;

[Service(ServiceLifeStyle.Singleton)]
public class MyPlayerService : IMyPlayerService
{
    private readonly ILogger<MyPlayerService> _logger;

    private int counter = 1;
    
    public MyPlayerService(ILogger<MyPlayerService> logger)
    {
        _logger = logger;
    }

    public void LogIt()
    {
        _logger.LogInformation("counter: {N}", counter);
        counter++;
    }
}
