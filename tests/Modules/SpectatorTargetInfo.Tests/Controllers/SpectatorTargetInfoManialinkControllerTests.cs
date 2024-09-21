using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Controllers;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Interfaces;
using EvoSC.Testing.Controllers;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Tests.Controllers;

public class
    SpectatorTargetInfoManialinkControllerTests : ManialinkControllerTestBase<SpectatorTargetInfoManialinkController>
{
    private Mock<ISpectatorTargetInfoService> _spectatorTargetService = new();
    private Mock<IManialinkActionContext> _manialinkActionContext = new();
    private Mock<IOnlinePlayer> _actor = new();

    public SpectatorTargetInfoManialinkControllerTests()
    {
        InitMock(_actor.Object, _manialinkActionContext.Object, _spectatorTargetService.Object);
    }

    [Fact]
    public async Task SetsSpectatorTargetIfGivenLoginIsValid()
    {
        _actor.Setup(actor => actor.AccountId)
            .Returns("*fakeplayer_spectator*");

        var spectatorLogin = _actor.Object.GetLogin();
        var targetLogin = "*fakeplayer_target*";

        await Controller.ReportSpectatorTargetAsync(targetLogin);

        _spectatorTargetService.Verify(st => st.SetSpectatorTargetAndSendAsync(spectatorLogin, targetLogin), Times.Once);
        _spectatorTargetService.Verify(st => st.RemovePlayerFromSpectatorsListAsync(spectatorLogin), Times.Never);
        _spectatorTargetService.Verify(st => st.HideSpectatorInfoWidgetAsync(spectatorLogin), Times.Never);
    }

    [Fact]
    public async Task RemoveSpectatorIfTargetIsEmpty()
    {
        _actor.Setup(actor => actor.AccountId)
            .Returns("*fakeplayer_spectator*");

        var spectatorLogin = _actor.Object.GetLogin();
        var targetLogin = "";

        await Controller.ReportSpectatorTargetAsync(targetLogin);

        _spectatorTargetService.Verify(st => st.SetSpectatorTargetAndSendAsync(spectatorLogin, targetLogin), Times.Never);
        _spectatorTargetService.Verify(st => st.RemovePlayerFromSpectatorsListAsync(spectatorLogin), Times.Once);
        _spectatorTargetService.Verify(st => st.HideSpectatorInfoWidgetAsync(spectatorLogin), Times.Once);
    }

    [Fact]
    public async Task RemoveSpectatorIfTargetIsSpectatorThemselvesEmpty()
    {
        _actor.Setup(actor => actor.AccountId)
            .Returns("*fakeplayer_spectator*");

        var spectatorLogin = _actor.Object.GetLogin();
        var targetLogin = "*fakeplayer_spectator*";

        await Controller.ReportSpectatorTargetAsync(targetLogin);

        _spectatorTargetService.Verify(st => st.SetSpectatorTargetAndSendAsync(spectatorLogin, targetLogin), Times.Never);
        _spectatorTargetService.Verify(st => st.RemovePlayerFromSpectatorsListAsync(spectatorLogin), Times.Once);
        _spectatorTargetService.Verify(st => st.HideSpectatorInfoWidgetAsync(spectatorLogin), Times.Once);
    }
}
