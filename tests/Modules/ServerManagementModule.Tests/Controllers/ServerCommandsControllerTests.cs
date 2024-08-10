using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.ServerManagementModule.Controllers;
using EvoSC.Modules.Official.ServerManagementModule.Events;
using EvoSC.Modules.Official.ServerManagementModule.Interfaces;
using EvoSC.Testing.Controllers;
using Moq;

namespace ServerManagementModule.Tests.Controllers;

public class ServerCommandsControllerTests : CommandInteractionControllerTestBase<ServerCommandsController>
{
    private readonly Mock<IServerManagementService> _serverManagementMock = new();
    private readonly Mock<IOnlinePlayer> _player = new();
    
    public ServerCommandsControllerTests()
    {
        InitMock(_player.Object, _serverManagementMock);
    }

    [Fact]
    public async Task SetServerPassword_Calls_Service_And_Audits()
    {
        const string Password = "MyPassword123";
        await Controller.SetServerPasswordAsync(Password);
        
        _serverManagementMock.Verify(m => m.SetPasswordAsync(Password));
        AuditEventBuilder.Verify(m => m.WithEventName(ServerManagementAuditEvents.PasswordSet));
        AuditEventBuilder.Verify(m => m.Success());
    }
    
    [Fact]
    public async Task ClearPassword_Sets_Empty_Password_And_Audits()
    {
        await Controller.ClearServerPasswordAsync();
        
        _serverManagementMock.Verify(m => m.SetPasswordAsync(""));
        AuditEventBuilder.Verify(m => m.WithEventName(ServerManagementAuditEvents.PasswordSet));
        AuditEventBuilder.Verify(m => m.Success());
    }
    
    [Fact]
    public async Task SetMaxPlayers_Calls_Service_And_Audits()
    {
        const int MaxPlayers = 123;
        await Controller.SetMaxPlayersAsync(MaxPlayers);
        
        _serverManagementMock.Verify(m => m.SetMaxPlayersAsync(MaxPlayers));
        AuditEventBuilder.Verify(m => m.WithEventName(ServerManagementAuditEvents.MaxPlayersSet));
        AuditEventBuilder.Verify(m => m.Success());
    }
    
    [Fact]
    public async Task SetMaxSpectators_Calls_Service_And_Audits()
    {
        const int MaxPlayers = 123;
        await Controller.SetMaxSpectatorsAsync(MaxPlayers);
        
        _serverManagementMock.Verify(m => m.SetMaxSpectatorsAsync(MaxPlayers));
        AuditEventBuilder.Verify(m => m.WithEventName(ServerManagementAuditEvents.MaxSpectatorsSet));
        AuditEventBuilder.Verify(m => m.Success());
    }
}
