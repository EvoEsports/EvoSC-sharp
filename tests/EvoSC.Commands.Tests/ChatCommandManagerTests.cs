using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using Microsoft.Extensions.Logging;
using Xunit;

namespace EvoSC.Commands.Tests;

public class ChatCommandManagerTests
{
    [Controller]
    public class MyController : EvoScController<IControllerContext>
    {
        
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

        var cmdManager = new ChatCommandManager(new Logger<ChatCommandManager>(new LoggerFactory()), null);
        
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

        var cmdManager = new ChatCommandManager(new Logger<ChatCommandManager>(new LoggerFactory()), null);
        
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

        var cmdManager = new ChatCommandManager(new Logger<ChatCommandManager>(new LoggerFactory()), null);
        
        cmdManager.AddCommand(cmd);

        var foundCmd = cmdManager.FindCommand("myAlias");
        
        Assert.NotNull(foundCmd);
    }
}
