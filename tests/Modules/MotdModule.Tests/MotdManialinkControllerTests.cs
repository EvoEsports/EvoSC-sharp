using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.MotdModule.Controllers;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Testing.Controllers;
using Moq;

namespace MotdModule.Tests;

public class MotdManialinkControllerTests : ManialinkControllerTestBase<MotdManialinkController>
{
    private Mock<IManialinkActionContext> _manialinkActionContext = new();
    private Mock<IOnlinePlayer> _actor = new();
    private Mock<IMotdService> _motdService = new();

    public MotdManialinkControllerTests()
    {
        InitMock(_actor.Object, _manialinkActionContext.Object, _motdService.Object);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    async Task CloseTest(bool hidden)
    {
        await Controller.CloseAsync(hidden);
        
        ManialinkManager.Verify(m => m.HideManialinkAsync(_actor.Object, "MotdModule.MotdTemplate"));
        _motdService.Verify(r => r.InsertOrUpdateEntryAsync(_actor.Object, hidden));
    }
}
