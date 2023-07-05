using System;
using System.Collections.Generic;
using EvoSC.Common.Config;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Config.Stores;
using Xunit;

namespace EvoSC.Common.Tests.Config.Stores;

public class EvoScBaseConfigStoreTests
{
    private static string ConfigFile = "TestFiles/Config/BasicConfig/main.toml";

    [Fact]
    public void Can_Read()
    {
        var store = new EvoScBaseConfigStore(ConfigFile, new Dictionary<string, string>());
        
        Assert.True(store.CanRead);
    }
    
    [Fact]
    public void Cant_Write()
    {
        var store = new EvoScBaseConfigStore(ConfigFile, new Dictionary<string, string>());
        
        Assert.False(store.CanWrite);
        Assert.Throws<NotSupportedException>(() => store.Write("test", "test"));
    }

    [Fact]
    public void Environment_Variables_Overrides_FileConfig_Simple_Option()
    {
        Environment.SetEnvironmentVariable("EVOSC_DATABASE_TYPE", "MySql");
        var config = Configuration.GetBaseConfig(ConfigFile, new Dictionary<string, string>());

        Assert.Equal(IDatabaseConfig.DatabaseType.MySql, config.Database.Type);
    }

    [Fact]
    public void Environment_Variables_Overrides_FileConfig_Array_Option()
    {
        Environment.SetEnvironmentVariable("EVOSC_MODULES_MODULEDIRECTORIES", "dir1 dir2");
        var config = Configuration.GetBaseConfig(ConfigFile, new Dictionary<string, string>());

        Assert.Equal(new []{"dir1", "dir2"}, config.Modules.ModuleDirectories);
    }

    [Fact]
    public void Environment_Variables_Overrides_FileConfig_Boolean_Option()
    {
        Environment.SetEnvironmentVariable("EVOSC_MODULES_REQUIRESIGNATUREVERIFICATION", "true");
        var config = Configuration.GetBaseConfig(ConfigFile, new Dictionary<string, string>());

        Assert.True(config.Modules.RequireSignatureVerification);
    }

    [Fact]
    public void Environment_Variables_Overrides_FileConfig_CustomType_Option()
    {
        Environment.SetEnvironmentVariable("EVOSC_THEME_CHAT_PRIMARYTEXTCOLOR", "745");
        var config = Configuration.GetBaseConfig(ConfigFile, new Dictionary<string, string>());

        Assert.Equal("$745", config.Theme.Chat.PrimaryColor.ToString());
    }
    
    
    [Fact]
    public void CliOption_Overrides_FileConfig_Simple_Option()
    {
        var config = Configuration.GetBaseConfig(ConfigFile, new Dictionary<string, string>()
        {
            {"Database.type", "MySql"}
        });

        Assert.Equal(IDatabaseConfig.DatabaseType.MySql, config.Database.Type);
    }

    [Fact]
    public void CliOption_Variables_Overrides_FileConfig_Array_Option()
    {
        var config = Configuration.GetBaseConfig(ConfigFile, new Dictionary<string, string>()
        {
            {"Modules.moduleDirectories", "dir1 dir2"}
        });

        Assert.Equal(new []{"dir1", "dir2"}, config.Modules.ModuleDirectories);
    }

    [Fact]
    public void CliOption_Variables_Overrides_FileConfig_Boolean_Option()
    {
        var config = Configuration.GetBaseConfig(ConfigFile, new Dictionary<string, string>()
        {
            {"Modules.requireSignatureVerification", "true"}
        });

        Assert.True(config.Modules.RequireSignatureVerification);
    }

    [Fact]
    public void CliOption_Variables_Overrides_FileConfig_CustomType_Option()
    {
        var config = Configuration.GetBaseConfig(ConfigFile, new Dictionary<string, string>()
        {
            {"Theme.Chat.primaryTextColor", "745"}
        });

        Assert.Equal("$745", config.Theme.Chat.PrimaryColor.ToString());
    }

    [Fact]
    public void CliOption_Overrides_Environment_Variable()
    {
        Environment.SetEnvironmentVariable("EVOSC_DATABASE_TYPE", "MySql");
        var config = Configuration.GetBaseConfig(ConfigFile, new Dictionary<string, string>()
        {
            {"Database.type", "SQLite"}
        });
        
        Assert.Equal(IDatabaseConfig.DatabaseType.SQLite, config.Database.Type);
    }
}
