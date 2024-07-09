using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.LocalRecordsModule.Controllers;
using EvoSC.Testing.Controllers;
using Moq;

namespace LocalRecordsModule.Tests.Controllers;

public class CommandsControllerTests : CommandInteractionControllerTestBase<CommandsController>
{
    private Mock<IOnlinePlayer> _actor = new();
    private Mock<IManialinkManager> _manialinkManager = new();
    
    public CommandsControllerTests()
    {
        InitMock(_actor.Object, _manialinkManager);
    }

    [Fact]
    public async Task Reset_Locals_Is_Confirmed()
    {
        await Controller.ResetLocalsAsync();
        
        _manialinkManager.Verify(m => m.SendManialinkAsync(_actor.Object, "LocalRecordsModule.Dialogs.ConfirmResetDialog"));
        AuditEventBuilder.Verify(m => m.Cancel());
    }
}
