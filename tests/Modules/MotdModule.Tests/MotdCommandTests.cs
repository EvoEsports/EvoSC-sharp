using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MotdModule.Controllers;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Modules.Official.Player.Controllers;
using EvoSC.Modules.Official.Player.Interfaces;
using EvoSC.Testing.Controllers;
using Moq;

namespace MotdModule.Tests;

public class MotdCommandTests : CommandInteractionControllerTestBase<MotdCommandController>
{
    private Mock<IMotdService> _motdService = new();
    private Mock<IOnlinePlayer> _player = new();
    Mock<IManialinkManager> _maniaLinkManager = new();

    
    public MotdCommandTests()
    {
        InitMock(_motdService);
    }
    
    [Fact]
    public async Task OpenMotdTest()
    {
        await Controller.OpenMotd();
        
        _motdService.Verify(r => r.ShowAsync(It.IsAny<IOnlinePlayer>()), Times.Once);
    }
    
    [Fact]
    public void SetUrlTest()
    {
        Controller.SetUrlAsync("testing");
        
        _motdService.Verify(r => r.SetUrl(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public void SetIntervalTest()
    {
        Controller.SetFetchIntervalAsync(1000);
        
        _motdService.Verify(r => r.SetInterval(It.IsAny<int>()), Times.Once);
    }
}
