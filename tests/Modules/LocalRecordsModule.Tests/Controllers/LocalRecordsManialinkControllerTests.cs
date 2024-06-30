using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.LocalRecordsModule.Controllers;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using Moq;

namespace LocalRecordsModule.Tests.Controllers;

public class LocalRecordsManialinkControllerTests : ManialinkControllerTestBase<LocalRecordsManialinkController>
{
    private readonly Mock<IManialinkActionContext> _manialinkActionContext = new();
    private readonly Mock<IOnlinePlayer> _actor = new();
    private readonly Mock<ILocalRecordsService> _localRecordsService = new();
    private readonly (Mock<IServerClient> Server, Mock<IGbxRemoteClient> Remote)
        _server = Mocking.NewServerClientMock();

    public LocalRecordsManialinkControllerTests()
    {
        InitMock(_actor.Object, _manialinkActionContext.Object, _localRecordsService, _server.Server);
    }

    [Fact]
    public async Task Reset_Records_Confirmation_Resets_All_Records()
    {
        await Controller.ConfirmResetAsync(true);
        
        ManialinkManager.Verify(m => m.HideManialinkAsync("LocalRecordsModule.Dialogs.ConfirmResetDialog"));
        _localRecordsService.Verify(m => m.ResetLocalRecordsAsync());
        AuditEventBuilder.Verify(m => m.Success());
    }
    
    [Fact]
    public async Task Reset_Records_Cancel_Does_Not_Resets_All_Records()
    {
        await Controller.ConfirmResetAsync(false);
        
        ManialinkManager.Verify(m => m.HideManialinkAsync("LocalRecordsModule.Dialogs.ConfirmResetDialog"));
        _localRecordsService.Verify(m => m.ResetLocalRecordsAsync(), Times.Never);
        AuditEventBuilder.Verify(m => m.Success(), Times.Never);
        AuditEventBuilder.Verify(m => m.Error(), Times.Never);
    }

    [Fact]
    public async Task Reset_Records_Error_Is_Reported()
    {
        _localRecordsService.Setup(m => m.ResetLocalRecordsAsync()).Throws<Exception>();
        
        await Assert.ThrowsAsync<Exception>(() => Controller.ConfirmResetAsync(true));
        _server.Server.Verify(m => m.ErrorMessageAsync(It.IsAny<string>()));
        AuditEventBuilder.Verify(m => m.Error());
    }
}
