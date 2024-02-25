using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.MotdModule.Controllers;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Modules.Official.MotdModule.Models;
using EvoSC.Testing.Controllers;
using NSubstitute;

namespace MotdModule.Tests;

public class MotdEditManialinkControllerTests : ManialinkControllerTestBase<MotdEditManialinkController>
{
    private readonly IManialinkActionContext _manialinkActionContext = Substitute.For<IManialinkActionContext>();
    private readonly IOnlinePlayer _actor = Substitute.For<IOnlinePlayer>();
    private readonly IMotdService _motdService = Substitute.For<IMotdService>();

    public MotdEditManialinkControllerTests()
    {
        InitMock(_actor, _manialinkActionContext, _motdService);
    }

    [Fact]
    public async Task SaveAsync_Closes_Manialink_And_Sets_LocalMotd()
    {
        await Controller.SaveAsync(new EditMotdEntryModel { Text = "testing stuff" });

        ManialinkManager.Received().HideManialinkAsync(_actor, "MotdModule.MotdEdit");
        _motdService.Received().SetLocalMotd("testing stuff", Arg.Any<IPlayer>());
    }
}
