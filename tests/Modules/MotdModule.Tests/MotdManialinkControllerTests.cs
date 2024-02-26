using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.MotdModule.Controllers;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Testing.Controllers;
using NSubstitute;

namespace MotdModule.Tests;

public class MotdManialinkControllerTests : ManialinkControllerTestBase<MotdManialinkController>
{
    private readonly IManialinkActionContext _manialinkActionContext = Substitute.For<IManialinkActionContext>();
    private readonly IOnlinePlayer _actor = Substitute.For<IOnlinePlayer>();
    private readonly IMotdService _motdService = Substitute.For<IMotdService>();

    public MotdManialinkControllerTests()
    {
        InitMock(_actor, _manialinkActionContext, _motdService);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task CloseAsync_Closes_Manialink_And_Updates_MotdEntry(bool hidden)
    {
        await Controller.CloseAsync(hidden);
        
        await ManialinkManager.Received().HideManialinkAsync(_actor, "MotdModule.MotdTemplate");
        await _motdService.Received().InsertOrUpdateEntryAsync(_actor, hidden);
    }
}
