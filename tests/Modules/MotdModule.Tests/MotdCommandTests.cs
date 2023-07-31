using EvoSC.Commands.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MotdModule.Controllers;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using Moq;

namespace MotdModule.Tests;

public class MotdCommandTests : CommandInteractionControllerTestBase<MotdCommandController>
{
    private readonly Mock<IMotdService> _motdService = new();
    private readonly Mock<IContextService> _context;
    private readonly ControllerContextMock<ICommandInteractionContext> _commandContext = Mocking.NewControllerContextMock<ICommandInteractionContext>();
    
    public MotdCommandTests()
    {
        _context = Mocking.NewContextServiceMock(_commandContext.Context.Object, null);
        InitMock(_motdService, _context.Object);
    }

    [Theory]
    [InlineData("true")]
    [InlineData("false")]
    public void SetMotdLocal_Sets_Motd_Source_To_Local(string isLocal)
    {
        Controller.SetMotdLocal(isLocal);
        _motdService.Verify(r => r.SetMotdSource(bool.Parse(isLocal), null));
    }

    [Fact]
    public async Task OpenEditMotdAsync_Shows_Edit()
    {
        await Controller.OpenEditMotdAsync();
        _motdService.Verify(r => r.ShowEditAsync(It.IsAny<IPlayer>()));
    }
    
    [Fact]
    public async Task OpenMotdAsync_Shows_Motd()
    {
        await Controller.OpenMotdAsync();
        
        _motdService.Verify(r => r.ShowAsync(It.IsAny<IOnlinePlayer>(), It.IsAny<bool>()), Times.Once);
    }
    
    [Fact]
    public void SetUrl_Changes_Motd_Url()
    {
        Controller.SetUrl("testing");
        
        _motdService.Verify(r => r.SetUrl(It.IsAny<string>(), It.IsAny<IPlayer>()), Times.Once);
    }
    
    [Fact]
    public void SetInterval_Changes_MotdTimer_Interval()
    {
        Controller.SetFetchInterval(1000);
        
        _motdService.Verify(r => r.SetInterval(It.IsAny<int>(), It.IsAny<IPlayer>()), Times.Once);
    }
}
