using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.ForceTeamModule.Controllers;
using EvoSC.Modules.Official.ForceTeamModule.Events;
using EvoSC.Modules.Official.ForceTeamModule.Interfaces;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Structs;
using Moq;

namespace ForceTeamModule.Tests.Controllers;

public class ForceTeamManialinkControllerTests : ManialinkControllerTestBase<ForceTeamManialinkController>
{
    private const string PlayerAccountId = "a467a996-eba5-44bf-9e2b-8543b50f39ae";
    
    private readonly Mock<IForceTeamService> _forceTeamService = new();
    private readonly Mock<IPlayerManagerService> _playerManagerService = new();
    private readonly Mock<IOnlinePlayer> _player = new();
    private readonly Mock<IManialinkActionContext> _actionContext = new();
    
    public ForceTeamManialinkControllerTests()
    {
        _player.Setup(m => m.AccountId).Returns(PlayerAccountId);
        InitMock(_player.Object, _actionContext.Object, _forceTeamService, _playerManagerService);
    }

    [Fact]
    public async Task Player_Is_Switched_And_Audited()
    {
        _playerManagerService.Setup(m => m.GetOnlinePlayerAsync(PlayerAccountId)).ReturnsAsync(_player.Object);
        _forceTeamService.Setup(m => m.SwitchPlayerAsync(_player.Object)).ReturnsAsync(PlayerTeam.Team1);
        
        var serverMock = Mocking.NewServerClientMock();
        serverMock.Remote.Setup(m => m.GetTeamInfoAsync(1)).ReturnsAsync(new TmTeamInfo
        {
            Name = "Team", RGB = "12345678"
        });
        
        Context.Setup(m => m.Server).Returns(serverMock.Client.Object);
        
        await Controller.SwitchPlayerAsync(PlayerAccountId);
        
        _forceTeamService.Verify(m => m.SwitchPlayerAsync(_player.Object));
        AuditEventBuilder.Verify(m => m.WithEventName(AuditEvents.PlayerSwitched));
        AuditEventBuilder.Verify(m => m.Success());
    }

    [Fact]
    public async Task Player_Not_Found_Sends_Error()
    {
        _playerManagerService.Setup(m => m.GetOnlinePlayerAsync(PlayerAccountId)).ReturnsAsync((IOnlinePlayer?)null);

        await Controller.SwitchPlayerAsync(PlayerAccountId);

        Server.Chat.Verify(m => m.ErrorMessageAsync(It.IsAny<string>(), _player.Object));
    }
}
