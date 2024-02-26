using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MatchManagerModule.Controllers;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using EvoSC.Testing.Controllers;
using NSubstitute;

namespace MatchManagerModule.Tests.Controllers;

public class MatchSettingsCommandsControllerTests : CommandInteractionControllerTestBase<MatchSettingsCommandsController>
{
    private readonly IOnlinePlayer _player = Substitute.For<IOnlinePlayer>();
    private readonly IMatchManagerHandlerService _matchHandler = Substitute.For<IMatchManagerHandlerService>();

    public MatchSettingsCommandsControllerTests()
    {
        InitMock(_player, _matchHandler);
    }

    [Fact]
    public async Task Mode_Is_Set()
    {
        await Controller.SetModeAsync("MyMode");

        await _matchHandler.Received(1).SetModeAsync("MyMode", Context.Player);
    }
    
    [Fact]
    public async Task Match_Settings_Is_Loaded()
    {
        await Controller.LoadMatchSettingsAsync("MySettings");

        await _matchHandler.Received(1).LoadMatchSettingsAsync("MySettings", Context.Player);
    }
    
    [Fact]
    public async Task Script_Setting_Is_Set()
    {
        await Controller.SetScriptSettingAsync("S_MySetting", "123");

        await _matchHandler.Received(1).SetScriptSettingAsync("S_MySetting", "123", Context.Player);
    }
}
