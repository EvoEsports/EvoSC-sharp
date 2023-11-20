using System;
using System.IO;
using Config.Net;
using EvoSC.Common.Config.Mapping;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Config.Stores;
using Tomlet.Exceptions;
using Xunit;

namespace EvoSC.Common.Tests.Config.Stores;

public class TomlConfigStoreTests
{
    private static string ConfigFile = "TestFiles/Config/BasicConfig/main.toml";

    private TomlConfigStore<IEvoScBaseConfig> GetStore() => new TomlConfigStore<IEvoScBaseConfig>(ConfigFile);

    [Fact]
    public void Can_Read()
    {
        var store = GetStore();
        
        Assert.True(store.CanRead);
    }
    
    [Fact]
    public void Cant_Write()
    {
        var store = GetStore();
        
        Assert.False(store.CanWrite);
        Assert.Throws<NotSupportedException>(() => store.Write("test", "test"));
    }

    [Theory]
    [InlineData("Database.type", "PostgreSql")]
    [InlineData("Database.port", "1234")]
    [InlineData("Logging.useJson", "false")]
    [InlineData("Theme.Chat.primaryTextColor", "123")]
    [InlineData("Modules.moduleDirectories", "mymodules")]
    [InlineData("Modules.disabledModules", "mymodule mymodule2")]
    [InlineData("Modules.disabledModules[0]", "mymodule")]
    [InlineData("Modules.disabledModules[1]", "mymodule2")]
    public void Reads_Value_Correctly(string key, string? expected)
    {
        var store = GetStore();

        var value = store.Read(key);
        
        Assert.Equal(expected, value);
    }

    [Fact]
    public void Invalid_Key_Throws_Exception()
    {
        var store = GetStore();

        Assert.Throws<TomlNoSuchValueException>(() => store.Read("does.not.exist"));
    }

    [Fact]
    public void Config_File_Is_Created_If_Nonexistent()
    {
        var path = "TestFiles/Config/auto_created.toml";
        var store = new TomlConfigStore<IEvoScBaseConfig>(path);

        var fileExists = File.Exists(path);
        
        Assert.True(fileExists);
    }

    [Fact]
    public void Config_Directory_Is_Created_If_Noneexistent()
    {
        var path = "TestFiles/Config/AutoCreated";
        var store = new TomlConfigStore<IEvoScBaseConfig>(path + "/config.toml");

        var dirExists = Directory.Exists(path);
        
        Assert.True(dirExists);
    }

    [Fact]
    public void EvoSc_Config_Parsed_With_Store()
    {
        var config = new ConfigurationBuilder<IEvoScBaseConfig>()
            .UseConfigStore(new TomlConfigStore<IEvoScBaseConfig>(ConfigFile))
            .UseTypeParser(new TextColorTypeParser())
            .UseTypeParser(new ThemeOptionsParser())
            .Build();
        
        Assert.Equal(IDatabaseConfig.DatabaseType.PostgreSql, config.Database.Type);
        Assert.Equal("123.123.123.123", config.Database.Host);
        Assert.Equal(1234, config.Database.Port);
        Assert.Equal("testdb", config.Database.Name);
        Assert.Equal("testdbname", config.Database.Username);
        Assert.Equal("testdbpassword", config.Database.Password);
        Assert.Equal("testprefix", config.Database.TablePrefix);
        
        Assert.Equal("debug", config.Logging.LogLevel);
        Assert.False(config.Logging.UseJson);
        
        Assert.Equal("123.123.123.123", config.Server.Host);
        Assert.Equal(1234, config.Server.Port);
        Assert.Equal("TestAdmin", config.Server.Username);
        Assert.Equal("TestPassword", config.Server.Password);
        Assert.True(config.Server.RetryConnection);
        
        Assert.Equal("my/server/maps", config.Path.Maps);
        
        Assert.Equal("no", config.Locale.DefaultLanguage);
        
        Assert.False(config.Modules.RequireSignatureVerification);
        Assert.Equal(new []{"mymodules"}, config.Modules.ModuleDirectories);
        Assert.Equal(new []{"mymodule", "mymodule2"}, config.Modules.DisabledModules);
    }
}
