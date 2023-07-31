using System.Threading.Tasks;
using EvoSC.CLI.Attributes;
using EvoSC.Common.Application;
using EvoSC.Common.Config.Models;
using Xunit;

namespace EvoSC.CLI.Tests.TestClasses;

[CliCommand(Name = "Test", Description = "")]
[RequiredFeatures(AppFeature.Config)]
public class CliOptionsParsedCmdClass
{
    private readonly IEvoScBaseConfig _config;
    
    public CliOptionsParsedCmdClass(IEvoScBaseConfig config)
    {
        _config = config;
    }
    
    public Task ExecuteAsync()
    {
        Assert.Equal("123.456.789.012", _config.Server.Host);
        
        return Task.CompletedTask;
    }
}
