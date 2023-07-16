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
    private async Task SetMotdLocal_Test(string isLocal)
    {
        Controller.SetMotdLocal(isLocal);
        _motdService.Verify(r => r.SetMotdSource(bool.Parse(isLocal), null));
    }

    [Fact]
    private async Task OpenEditMotdAsync_Test()
    {
        await Controller.OpenEditMotdAsync();
        _motdService.Verify(r => r.ShowEditAsync(It.IsAny<IPlayer>()));
    }
    
    [Fact]
    private async Task OpenMotdAsync_Test()
    {
        await Controller.OpenMotdAsync();
        
        _motdService.Verify(r => r.ShowAsync(It.IsAny<IOnlinePlayer>(), It.IsAny<bool>()), Times.Once);
    }
    
    [Fact]
    private void SetUrl_Test()
    {
        Controller.SetUrl("testing");
        
        _motdService.Verify(r => r.SetUrl(It.IsAny<string>(), It.IsAny<IPlayer>()), Times.Once);
    }
    
    [Fact]
    private void SetInterval_Test()
    {
        Controller.SetFetchInterval(1000);
        
        _motdService.Verify(r => r.SetInterval(It.IsAny<int>(), It.IsAny<IPlayer>()), Times.Once);
    }
}
