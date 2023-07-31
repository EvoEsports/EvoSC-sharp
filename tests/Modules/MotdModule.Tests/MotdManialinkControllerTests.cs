using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.MotdModule.Controllers;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Testing.Controllers;
using Moq;

namespace MotdModule.Tests;

public class MotdManialinkControllerTests : ManialinkControllerTestBase<MotdManialinkController>
{
    private readonly Mock<IManialinkActionContext> _manialinkActionContext = new();
    private readonly Mock<IOnlinePlayer> _actor = new();
    private readonly Mock<IMotdService> _motdService = new();

    public MotdManialinkControllerTests()
    {
        InitMock(_actor.Object, _manialinkActionContext.Object, _motdService.Object);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task CloseAsync_Closes_Manialink_And_Updates_MotdEntry(bool hidden)
    {
        await Controller.CloseAsync(hidden);
        
        ManialinkManager.Verify(m => m.HideManialinkAsync(_actor.Object, "MotdModule.MotdTemplate"));
        _motdService.Verify(r => r.InsertOrUpdateEntryAsync(_actor.Object, hidden));
    }
}
