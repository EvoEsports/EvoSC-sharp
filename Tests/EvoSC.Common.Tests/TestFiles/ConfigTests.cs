using EvoSC.Common.Config;
using EvoSC.Common.Config.Models;
using Xunit;

namespace EvoSC.Common.Tests.TestFiles;

public class ConfigTests
{
    [Fact]
    public void Test_Server_Config_Is_Properly_Parsed()
    {
        var config = new EvoScConfig("TestFiles/Config/BasicConfig");
        
        var serverConfig = config.Get<ServerConfig>(EvoScConfig.ServerConfigKey);
        
        Assert.Equal("123.123.123.123", serverConfig.Host);
        Assert.Equal(1234, serverConfig.Port);
        Assert.Equal("My Admin ", serverConfig.Username);
        Assert.Equal(" My Admin", serverConfig.Password);
        Assert.False(serverConfig.RetryConnection);
    }
}
