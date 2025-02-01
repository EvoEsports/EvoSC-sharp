using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.SetNameModule.Controllers;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using Moq;

namespace EvoSC.Modules.Official.SetName.Tests;

public class SetNameCommandsControllerTests : CommandInteractionControllerTestBase<SetNameCommandsController>
{
    private Mock<IOnlinePlayer> _actor = new();
    private Mock<IManialinkManager> _manialinkManager = new();
    private Locale _locale;

    public SetNameCommandsControllerTests()
    {
        _locale = Mocking.NewLocaleMock(ContextService.Object);
        
        InitMock(_actor.Object, _manialinkManager, _locale);
    }
    
    [Fact]
    public async Task SetName_Dialog_Is_Opened()
    {
        await Controller.SetNameAsync();

        _manialinkManager.Verify(m =>
            m.SendManialinkAsync(_actor.Object, "SetNameModule.EditName", It.IsAny<It.IsAnyType>()));
    }
}
