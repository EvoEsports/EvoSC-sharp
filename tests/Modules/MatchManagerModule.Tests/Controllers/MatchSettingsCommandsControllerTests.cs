using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MatchManagerModule.Controllers;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using Moq;

namespace MatchManagerModule.Tests.Controllers;

public class MatchSettingsCommandsControllerTests : CommandInteractionControllerTestBase<MatchSettingsCommandsController>
{
    private Mock<IOnlinePlayer> _player = new();
    private Mock<IMatchManagerHandlerService> _matchHandler = new();

    public MatchSettingsCommandsControllerTests()
    {
        InitMock(_player.Object, _matchHandler);
    }

    [Fact]
    public async Task Mode_Is_Set()
    {
        await Controller.SetModeAsync("MyMode");

        _matchHandler.Verify(m => m.SetModeAsync("MyMode", Context.Object.Player), Times.Once);
    }
    
    [Fact]
    public async Task Match_Settings_Is_Loaded()
    {
        await Controller.LoadMatchSettingsAsync("MySettings");

        _matchHandler.Verify(m => m.LoadMatchSettingsAsync("MySettings", Context.Object.Player), Times.Once);
    }
    
    [Fact]
    public async Task Script_Setting_Is_Set()
    {
        await Controller.SetScriptSettingAsync("S_MySetting", "123");

        _matchHandler.Verify(m => m.SetScriptSettingAsync("S_MySetting", "123", Context.Object.Player), Times.Once);
    }
}
