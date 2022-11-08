using System;
using System.Net.Mime;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using Xunit;

namespace EvoSC.Commands.Tests;

public class ChatCommandManagerTests
{
    private ILogger<ChatCommandManager> _logger;
    private IEvoSCApplication _app;
    private IEventManager _events;

    [Controller]
    public class MyController : EvoScController<IControllerContext>
    {
        
    }

    public ChatCommandManagerTests()
    {
        _app = new Application(Array.Empty<string>());
        
        var eventLogger = LoggerFactory.Create(c => { }).CreateLogger<EventManager>();
        _events = new EventManager(eventLogger, _app, null);
    }
    
    [Fact]
    public void Command_Is_Added()
    {
        var cmd = new ChatCommandBuilder()
            .WithName("myCmd")
            .WithDescription("This is my command.")
            .WithController<MyController>()
            .WithHandlerMethod(() => {})
            .Build();

        var cmdManager = new ChatCommandManager(_logger);
        
        cmdManager.AddCommand(cmd);

        var foundCmd = cmdManager.FindCommand("myCmd");
        
        Assert.NotNull(foundCmd);
    }
    
    [Fact]
    public void NonExisting_Command_Returns_Null()
    {
        var cmd = new ChatCommandBuilder()
            .WithName("myCmd")
            .WithDescription("This is my command.")
            .WithController<MyController>()
            .WithHandlerMethod(() => {})
            .Build();

        var cmdManager = new ChatCommandManager(_logger);
        
        cmdManager.AddCommand(cmd);

        var foundCmd = cmdManager.FindCommand("nonExistingCommand");
        
        Assert.Null(foundCmd);
    }
    
    [Fact]
    public void Command_Found_By_Alias()
    {
        var cmd = new ChatCommandBuilder()
            .WithName("myCmd")
            .WithDescription("This is my command.")
            .WithController<MyController>()
            .WithHandlerMethod(() => {})
            .AddAlias("myAlias")
            .Build();

        var cmdManager = new ChatCommandManager(_logger);
        
        cmdManager.AddCommand(cmd);

        var foundCmd = cmdManager.FindCommand("myAlias");
        
        Assert.NotNull(foundCmd);
    }
}
