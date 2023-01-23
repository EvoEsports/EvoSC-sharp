using System.Threading.Tasks;
using EvoSC.Commands.Exceptions;
using EvoSC.Commands.Interfaces;
using EvoSC.Commands.Parser;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.TextParsing;
using EvoSC.Common.TextParsing.ValueReaders;
using Microsoft.Extensions.Logging;
using Xunit;

namespace EvoSC.Commands.Tests;

public class CommandParserTests
{
    private readonly IChatCommandManager _cmdManager;

    [Controller]
    public class MyController : EvoScController<IControllerContext>
    {
        
    }

    public CommandParserTests()
    {
        var cmd = new ChatCommandBuilder()
            .WithName("myCmd")
            .WithDescription("This is my command.")
            .WithController<MyController>()
            .WithHandlerMethod(() => {})
            .Build();

        _cmdManager = new ChatCommandManager(new Logger<ChatCommandManager>(new LoggerFactory()));
        _cmdManager.AddCommand(cmd);
    }

    ValueReaderManager GetFullValueReader()
    {
        var valueReader = new ValueReaderManager();
        valueReader.AddReader(new FloatReader());
        valueReader.AddReader(new IntegerReader());
        valueReader.AddReader(new StringReader());

        return valueReader;
    }

    [Fact]
    public async Task Simple_Command_Parsed()
    {
        var valueReader = GetFullValueReader();
        var parser = new ChatCommandParser(_cmdManager, valueReader);

        var result = await parser.ParseAsync("/myCmd");
        
        Assert.True(result.Success);
        Assert.Equal("myCmd", result.Command.Name);
    }

    [Fact]
    public async Task Unknown_Command_Is_Intended()
    {
        var valueReader = GetFullValueReader();
        var parser = new ChatCommandParser(_cmdManager, valueReader);
        
        var result = await parser.ParseAsync("/unknownCommand");
        
        Assert.False(result.Success);

        var ex = (CommandNotFoundException)result.Exception;
        Assert.True(ex.IntendedCommand);
    }
    
    [Fact]
    public async Task Unknown_Alias_Is_Unintended_Command()
    {
        var valueReader = GetFullValueReader();
        var parser = new ChatCommandParser(_cmdManager, valueReader);
        
        var result = await parser.ParseAsync("unknownAlias");
        
        Assert.False(result.Success);

        var ex = (CommandNotFoundException)result.Exception;
        Assert.False(ex.IntendedCommand);
    }
}
