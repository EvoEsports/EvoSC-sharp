using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.MotdModule.Controllers;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Testing.Controllers;
using Moq;

namespace MotdModule.Tests;

public class MotdEditManialinkControllerTests : ManialinkControllerTestBase<MotdEditManialinkController>
{
    private readonly Mock<IManialinkActionContext> _manialinkActionContext = new();
    private readonly Mock<IOnlinePlayer> _actor = new();
    private readonly Mock<IMotdService> _motdService = new();

    public MotdEditManialinkControllerTests()
    {
        InitMock(_actor.Object, _manialinkActionContext.Object, _motdService.Object);
    }

    [Fact]
    public async Task SaveAsync_Closes_Manialink_And_Sets_LocalMotd()
    {
        await Controller.SaveAsync("testing stuff");

        ManialinkManager.Verify(m => m.HideManialinkAsync(_actor.Object, "MotdModule.MotdEdit"));
        _motdService.Verify(r => r.SetLocalMotd("testing stuff", It.IsAny<IPlayer>()));
    }
}
