using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.SetName.Controllers;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using NSubstitute;

namespace EvoSC.Modules.Official.SetName.Tests;

public class SetNameCommandsControllerTests : CommandInteractionControllerTestBase<SetNameCommandsController>
{
    private readonly IOnlinePlayer _actor = Substitute.For<IOnlinePlayer>();
    private readonly IManialinkManager _manialinkManager = Substitute.For<IManialinkManager>();
    private readonly Locale _locale;

    public SetNameCommandsControllerTests()
    {
        _locale = Mocking.NewLocaleMock(ContextService);
        
        InitMock(_actor, _manialinkManager, _locale);
    }
    
    [Fact]
    public async Task SetName_Dialog_Is_Opened()
    {
        await Controller.SetNameAsync();

        await _manialinkManager.Received().SendManialinkAsync(_actor, "SetName.EditName", Arg.Any<object>());
    }
}
