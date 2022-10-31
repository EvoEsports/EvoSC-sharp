using Config.Net;
using EvoSC.Common.Config;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Config.Stores;
using Xunit;

namespace EvoSC.Common.Tests.TestFiles;

public class ConfigTests
{
    [Fact]
    public void Test_Server_Config_Is_Properly_Parsed()
    {
        var config = new ConfigurationBuilder<IEvoScBaseConfig>()
            .UseTomlFile("TestFiles/Config/BasicConfig/main.toml")
            .Build();
        
        Assert.Equal("123.123.123.123", config.Server.Host);
        Assert.Equal(1234, config.Server.Port);
        Assert.Equal("My Admin ", config.Server.Username);
        Assert.Equal(" My Admin", config.Server.Password);
        Assert.False(config.Server.RetryConnection);
    }
}
