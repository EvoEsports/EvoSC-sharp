using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Controllers;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Interfaces;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Events;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Tests.Controllers;

public class SpectatorTargetInfoEventControllerTests : ControllerMock<SpectatorTargetInfoEventController, IEventControllerContext>
{
    private Mock<ISpectatorTargetInfoService> _spectatorTargetService = new();

    public SpectatorTargetInfoEventControllerTests()
    {
        InitMock(_spectatorTargetService.Object);
    }

    [Fact]
    public async Task RemovesPlayerOnDisconnect()
    {
        var login = "*fakeplayer_unittest*";

        await Controller.OnPlayerDisconnect(null, new PlayerGbxEventArgs { Login = login });

        _spectatorTargetService.Verify(st => st.RemovePlayerFromSpectatorsListAsync(login), Times.Once);
    }
}
