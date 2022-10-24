using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.ExampleModule;

[Controller]
public class ExampleController : EvoScController<IControllerContext>
{
    private readonly ILogger<ExampleController> _logger;
    private readonly IServerClient _server;

    public ExampleController(ILogger<ExampleController> logger, IServerClient server)
    {
        _logger = logger;
        _server = server;
    }

    [Subscribe(GbxRemoteEvent.PlayerChat, IsAsync = true)]
    public async Task OnPlayerChat1(object sender, PlayerChatEventArgs args)
    {
        _logger.LogInformation("hello 1");
    }
    
    [Subscribe(GbxRemoteEvent.PlayerChat, IsAsync = true)]
    public async Task OnPlayerChat2(object sender, PlayerChatEventArgs args)
    {
        _logger.LogInformation("hello 2");
    }
    
    [Subscribe(GbxRemoteEvent.PlayerChat, IsAsync = true)]
    public async Task OnPlayerChat3(object sender, PlayerChatEventArgs args)
    {
        _logger.LogInformation("hello 3");
    }
    
    [Subscribe(GbxRemoteEvent.PlayerChat, IsAsync = true)]
    public async Task OnPlayerChat4(object sender, PlayerChatEventArgs args)
    {
        _logger.LogInformation("hello 4");
    }
    
    [Subscribe(GbxRemoteEvent.PlayerChat, IsAsync = false)]
    public async Task OnPlayerChat5(object sender, PlayerChatEventArgs args)
    {
        _logger.LogInformation("hello 5");
    }
    
    [Subscribe(GbxRemoteEvent.PlayerChat, IsAsync = false)]
    public async Task OnPlayerChat6(object sender, PlayerChatEventArgs args)
    {
        _logger.LogInformation("hello 6");
    }
    
    [Subscribe(GbxRemoteEvent.PlayerChat, EventPriority.High, IsAsync = false)]
    public async Task OnPlayerChat7(object sender, PlayerChatEventArgs args)
    {
        _logger.LogInformation("hello 7");
    }
    
    [Subscribe(GbxRemoteEvent.PlayerChat, EventPriority.High, IsAsync = false)]
    public async Task OnPlayerChat8(object sender, PlayerChatEventArgs args)
    {
        _logger.LogInformation("hello 8");
    }
}
