using EvoSC.Commands.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MotdModule.Controllers;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using NSubstitute;

namespace MotdModule.Tests;

public class MotdCommandTests : CommandInteractionControllerTestBase<MotdCommandController>
{
    private readonly IMotdService _motdService = Substitute.For<IMotdService>();
    private readonly IContextService _context;
    private readonly ControllerContextMock<ICommandInteractionContext> _commandContext = Mocking.NewControllerContextMock<ICommandInteractionContext>();
    
    public MotdCommandTests()
    {
        _context = Mocking.NewContextServiceMock(_commandContext.Context, null);
        InitMock(_motdService);
    }

    [Theory]
    [InlineData("true")]
    [InlineData("false")]
    public void SetMotdLocal_Sets_Motd_Source_To_Local(string isLocal)
    {
        Controller.SetMotdLocal(isLocal);
        _motdService.Received().SetMotdSource(bool.Parse(isLocal), Arg.Any<IPlayer>());
    }

    [Fact]
    public async Task OpenEditMotdAsync_Shows_Edit()
    {
        await Controller.OpenEditMotdAsync();
        await _motdService.Received().ShowEditAsync(Arg.Any<IPlayer>());
    }
    
    [Fact]
    public async Task OpenMotdAsync_Shows_Motd()
    {
        await Controller.OpenMotdAsync();
        
        await _motdService.Received(1).ShowAsync(Arg.Any<IOnlinePlayer>(), Arg.Any<bool>());
    }
    
    [Fact]
    public void SetUrl_Changes_Motd_Url()
    {
        Controller.SetUrl("testing");
        
        _motdService.Received(1).SetUrl(Arg.Any<string>(), Arg.Any<IPlayer>());
    }
    
    [Fact]
    public void SetInterval_Changes_MotdTimer_Interval()
    {
        Controller.SetFetchInterval(1000);
        
        _motdService.Received(1).SetInterval(Arg.Any<int>(), Arg.Any<IPlayer>());
    }
}
