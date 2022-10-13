using System;
using System.Threading;
using System.Threading.Tasks;
using EvoSC.CLI.Attributes;
using EvoSC.CLI.Exceptions;
using EvoSC.CLI.Interfaces;
using Xunit;

namespace EvoSC.CLI.Tests;

public class CliHandlerTests
{
    class InvalidCmdClass : ICliCommand
    {
        public Task ExecuteAsync(CancellationToken cancelToken, CliCommandContext context)
        {
            throw new System.NotImplementedException();
        }
    }

    class MyCommandRanException : Exception {}
    
    [CliCommand("mycmd", "My command.")]
    class MyCmd : ICliCommand
    {
        public Task ExecuteAsync(CancellationToken cancelToken, CliCommandContext context)
        {
            throw new MyCommandRanException();
        }
    }
    
    [Fact]
    public void Registration_Throws_Exception_If_Attribute_Is_Not_Defined()
    {
        var handler = new CliHandler(Array.Empty<string>());

        Assert.Throws<CliCommandAttributeNotFound>(() =>
        {
            handler.RegisterCommand(new InvalidCmdClass());
        });
    }

    [Fact]
    public void Handler_Runs_Registered_Command()
    {
        var handler = new CliHandler(new[] {"mycmd"});
        handler.RegisterCommand(new MyCmd());

        Assert.ThrowsAsync<MyCommandRanException>(async () =>
        {
            await handler.Handle();
        });
    }
}
