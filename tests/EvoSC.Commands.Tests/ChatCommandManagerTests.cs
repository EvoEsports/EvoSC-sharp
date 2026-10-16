using EvoSC.Commands.Attributes;
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

    [Controller]
    public class ControllerWithAliasedCommand : EvoScController<IControllerContext>
    {
        [ChatCommand("ratemap", "Rate the current map.")]
        [CommandAlias("+++")]
        public void RateMap()
        {
        }
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

    /// <summary>
    /// Regression test: re-registering a controller (e.g. a module re-enabled after being disabled)
    /// used to leave the previous registration's commands in the controller's tracked list instead
    /// of clearing it. On the next unregister, removing the stale entry (same name, different
    /// instance) deleted the live command by name, and the loop then failed to find it again for the
    /// entry that actually represented it - throwing CommandNotFoundException.
    /// </summary>
    [Fact]
    public void Controller_Can_Be_Registered_Unregistered_And_Reregistered_Repeatedly()
    {
        var cmdManager = new ChatCommandManager(new Logger<ChatCommandManager>(new LoggerFactory()), null);

        for (var cycle = 0; cycle < 3; cycle++)
        {
            cmdManager.RegisterForController(typeof(ControllerWithAliasedCommand));

            Assert.NotNull(cmdManager.FindCommand("ratemap"));
            Assert.NotNull(cmdManager.FindCommand("+++", withPrefix: false));

            cmdManager.UnregisterForController(typeof(ControllerWithAliasedCommand));

            Assert.Null(cmdManager.FindCommand("ratemap"));
            Assert.Null(cmdManager.FindCommand("+++", withPrefix: false));
        }
    }
}
