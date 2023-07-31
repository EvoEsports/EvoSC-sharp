using System;
using System.Threading.Tasks;
using EvoSC.CLI.Exceptions;
using EvoSC.CLI.Tests.TestClasses;
using Xunit;

namespace EvoSC.CLI.Tests;

public class CliManagerTests
{
    [Fact]
    public void Register_Command_Without_Handler_Throws_Exception()
    {
        var manager = new CliManager();

        Assert.Throws<InvalidCommandClassFormatException>(() =>
            manager.RegisterCommand(typeof(InvalidCommandClass))
        );
    }

    [Fact]
    public void No_Attribute_Throws_Exception()
    {
        var manager = new CliManager();

        Assert.Throws<InvalidCommandClassFormatException>(() => 
            manager.RegisterCommand(typeof(NoAttributeCmdClass))
        );
    }

    [Fact]
    public async Task Simple_Command_Registered_And_Executed()
    {
        var manager = new CliManager();
        manager.RegisterCommand(typeof(TestCommandClass));

        var exitCode = await manager.ExecuteAsync(new[] {"Test", "--myarg", "1337"});
        
        Assert.Equal(1337, exitCode);
    }

    [Fact]
    public async Task Invalid_Cli_Option_Format_Throws_Exception()
    {
        var manager = new CliManager();
        manager.RegisterCommand(typeof(TestCommandClass));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            manager.ExecuteAsync(new[] {"Test", "--myarg", "1337", "--option", "invalidformat"})
        );
    }

    [Fact]
    public async Task Config_Option_Is_Parsed_And_Overrides_Default()
    {
        var manager = new CliManager();
        manager.RegisterCommand(typeof(CliOptionsParsedCmdClass));

        await manager.ExecuteAsync(new[] {"Test", "--option", "Server.host:123.456.789.012"});
        
        // assert statement in CliOptionsParsedCmdClass ExecuteAsync method
    }
}
