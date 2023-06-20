using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvoSC.Common.Exceptions.Parsing;
using EvoSC.Common.Exceptions.PlayerExceptions;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Parsing;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Players;
using EvoSC.Common.TextParsing;
using EvoSC.Common.TextParsing.ValueReaders;
using Moq;
using Xunit;

namespace EvoSC.Common.Tests.Util;

public class ValueReaderManagerTests
{
    class TestReader : IValueReader
    {
        public IEnumerable<Type> AllowedTypes => new[] {typeof(string)};
        
        public Task<object> ReadAsync(Type type, string input)
        {
            return Task.FromResult((object)$"Success: {input}");
        }
    }

    class ExceptionTestReader : IValueReader
    {
        public IEnumerable<Type> AllowedTypes => new[] {typeof(string)};
        public Task<object> ReadAsync(Type type, string input)
        {
            throw new ValueConversionException();
        }
    }
    
    [Fact]
    public async Task Reader_Added()
    {
        var manager = new ValueReaderManager();
        manager.AddReader(new TestReader());

        var value = await manager.ConvertValueAsync<string>("something");
        
        Assert.Equal("Success: something", value);
    }

    [Fact]
    public async Task Reader_Removed()
    {
        var manager = new ValueReaderManager();
        manager.AddReader(new TestReader());
        manager.RemoveReaders(typeof(string));

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await manager.ConvertValueAsync<string>("something")
        );
    }

    [Fact]
    public async Task Throws_Formatting_Error_When_Parsing_Fails()
    {
        var manager = new ValueReaderManager();
        manager.AddReader(new ExceptionTestReader());

        await Assert.ThrowsAsync<FormatException>(async () =>
            await manager.ConvertValueAsync<string>("something")
        );
    }

    [Fact]
    public async Task Readers_Added_From_Constructor()
    {
        var manager = new ValueReaderManager(new TestReader());
        
        var value = await manager.ConvertValueAsync<string>("Test");
        
        Assert.Equal("Success: Test", value);
    }

    [Fact]
    public async Task Boolean_Reader_Correctly_Selected_For_Parsing_By_Manager()
    {
        var manager = new ValueReaderManager(new BooleanReader());
        
        bool value = await manager.ConvertValueAsync<bool>("1");

        Assert.True(value);
    }
    
    [Theory]
    [InlineData("1", true)]
    [InlineData("0", false)]
    [InlineData("yes", true)]
    [InlineData("no", false)]
    [InlineData("YeS", true)]
    [InlineData("nO", false)]
    [InlineData("true", true)]
    [InlineData("false", false)]
    [InlineData("TRuE", true)]
    [InlineData("FaLsE", false)]
    [InlineData("on", true)]
    [InlineData("off", false)]
    [InlineData("On", true)]
    [InlineData("oFF", false)]
    public async Task Boolean_Reader_Parses_Correctly(string input, bool expected)
    {
        var reader = new BooleanReader();

        var value = await reader.ReadAsync(typeof(bool), input) as bool?;
        
        Assert.NotNull(value);
        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("abcdef")]
    [InlineData("falsee")]
    [InlineData("11")]
    [InlineData("00")]
    public async Task Boolean_Reader_Fails_On_Invalid_Value(string input)
    {
        var reader = new BooleanReader();

        await Assert.ThrowsAsync<ValueConversionException>(async () =>
            await reader.ReadAsync(typeof(bool), input)
        );
    }
    
    [Fact]
    public async Task String_Reader_Correctly_Selected_For_Parsing_By_Manager()
    {
        var manager = new ValueReaderManager(new StringReader());

        var value = await manager.ConvertValueAsync<string>("Test");

        Assert.Equal("Test", value);
    }
    
    [Theory]
    [InlineData("", "")]
    [InlineData("adfhgafdgh", "adfhgafdgh")]
    public async Task String_Reader_Parses_Correctly(string input, string expected)
    {
        var reader = new StringReader();

        var value = await reader.ReadAsync(typeof(string), input) as string;
        
        Assert.NotNull(value);
        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(null)]
    public async Task String_Reader_Fails_On_Invalid_Value(string input)
    {
        var reader = new BooleanReader();

        await Assert.ThrowsAsync<ValueConversionException>(async () =>
            await reader.ReadAsync(typeof(string), input)
        );
    }
    
    [Theory]
    [InlineData(typeof(float))]
    [InlineData(typeof(double))]
    public async Task Float_Reader_Correctly_Selected_For_Parsing_By_Manager(Type type)
    {
        var expected = Convert.ChangeType(1.5, type);
        var manager = new ValueReaderManager(new FloatReader());

        var value = await manager.ConvertValueAsync(type, "1.5");

        Assert.Equal(expected, value);
    }
    
    [Theory]
    [InlineData("1", 1.0f)]
    [InlineData("1.0", 1.0f)]
    [InlineData(".1", 0.1f)]
    [InlineData("847.37826", 847.37826f)]
    [InlineData("-847.37826", -847.37826f)]
    [InlineData("0", 0f)]
    [InlineData("-0", 0f)]
    [InlineData("0.0000001", .0000001f)]
    [InlineData("345346", 345346.0f)]
    public async Task Float_Reader_Parses_Correctly(string input, float expected)
    {
        var reader = new FloatReader();

        var value = await reader.ReadAsync(typeof(float), input) as float?;
        
        Assert.NotNull(value);
        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("invalid")]
    [InlineData(".")]
    [InlineData("")]
    public async Task Float_Reader_Fails_On_Invalid_Value(string input)
    {
        var reader = new FloatReader();

        await Assert.ThrowsAsync<ValueConversionException>(async () =>
            await reader.ReadAsync(typeof(float), input)
        );
    }
    
    [Theory]
    [InlineData(typeof(int))]
    [InlineData(typeof(uint))]
    [InlineData(typeof(long))]
    [InlineData(typeof(ulong))]
    public async Task Integer_Reader_Correctly_Selected_For_Parsing_By_Manager(Type type)
    {
        var expected = Convert.ChangeType(123, type);
        var manager = new ValueReaderManager(new IntegerReader());

        var value = await manager.ConvertValueAsync(type, "123");
        
        Assert.Equal(expected, value);
    }
    
    [Theory]
    [InlineData("1", (int)1)]
    [InlineData("-1", (int)-1)]
    [InlineData("2143621", (int)2143621)]
    [InlineData("1", (uint)1)]
    [InlineData("2143621", (uint)2143621)]
    [InlineData("1", (long)1)]
    [InlineData("-1", (long)-1)]
    [InlineData("2143621", (long)2143621)]
    [InlineData("1", (ulong)1)]
    [InlineData("2143621", (ulong)2143621)]
    public async Task Integer_Reader_Parses_Correctly(string input, object expected)
    {
        var reader = new IntegerReader();

        var value = await reader.ReadAsync(expected.GetType(), input);
        
        Assert.NotNull(value);
        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("invalid")]
    [InlineData(".")]
    [InlineData("")]
    [InlineData("1.0")]
    public async Task Integer_Reader_Fails_On_Invalid_Value(string input)
    {
        var reader = new IntegerReader();

        await Assert.ThrowsAsync<ValueConversionException>(async () =>
            await reader.ReadAsync(typeof(int), input)
        );
    }
    
    
    [Fact]
    public async Task OnlinePlayer_Reader_Correctly_Selected_For_Parsing_By_Manager()
    {
        var onlinePlayer = new OnlinePlayer
        {
            Id = 0,
            AccountId = null,
            NickName = null,
            UbisoftName = null,
            Zone = null,
            State = PlayerState.Spectating,
            Flags = null
        };

        var playerManager = new Mock<IPlayerManagerService>();
        playerManager.Setup(m => m.FindOnlinePlayerAsync("player"))
            .Returns(() => Task.FromResult(new IOnlinePlayer[] {onlinePlayer}.AsEnumerable()));
        
        var manager = new ValueReaderManager(new OnlinePlayerReader(playerManager.Object));

        var value = await manager.ConvertValueAsync(typeof(IOnlinePlayer), "player") as IOnlinePlayer;
        
        Assert.NotNull(value);
        Assert.Equal(onlinePlayer, value);
    }

    [Fact]
    public async Task OnlinePlayer_Reader_Not_Finding_Players_Throws_Exception()
    {
        var playerManager = new Mock<IPlayerManagerService>();
        playerManager.Setup(m => m.FindOnlinePlayerAsync("player"))
            .Returns(() => Task.FromResult(Array.Empty<IOnlinePlayer>().AsEnumerable()));

        var reader = new OnlinePlayerReader(playerManager.Object);

        await Assert.ThrowsAsync<PlayerNotFoundException>(() =>
            reader.ReadAsync(typeof(IOnlinePlayer), "")
        );
    }
}
