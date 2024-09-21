using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Official.GameModeUiModule.Config;
using EvoSC.Modules.Official.GameModeUiModule.Interfaces;
using EvoSC.Modules.Official.GameModeUiModule.Models;
using EvoSC.Modules.Official.GameModeUiModule.Services;
using EvoSC.Testing;
using GbxRemoteNet.Interfaces;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace EvoSC.Modules.Official.GameModeUiModule.Tests.Services;

public class GameModeUiModuleServiceTests
{
    private readonly Mock<IGameModeUiModuleSettings> _settings = new();

    private readonly (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote, Mock<IChatService> Chat)
        _server = Mocking.NewServerClientMock();

    private IGameModeUiModuleService UiModuleServiceMock()
    {
        return new GameModeUiModuleService(_server.Client.Object, _settings.Object);
    }

    [Theory]
    [InlineData("UnitTest", true, 0.0, 0.0, 1.0)]
    [InlineData("UnitTest", false, -160.0, 80.0, 2.0)]
    [InlineData("UnitTest", false, 160.0, -80.0, 0.5)]
    public Task Generates_Property_Object(string id, bool visible, double x, double y, double scale)
    {
        var uiModuleService = UiModuleServiceMock();
        var uiModulePropertyObject =
            uiModuleService.GeneratePropertyObject(new GameModeUiComponentSettings(id, visible, x, y, scale));

        var idProperty = uiModulePropertyObject.GetType().GetProperty("id");
        Assert.Equal(id, idProperty.GetValue(uiModulePropertyObject, null));

        var positionProperty = uiModulePropertyObject.GetType().GetProperty("position");
        var positionValue = (double[])positionProperty.GetValue(uiModulePropertyObject, null);
        Assert.Equal(2, positionValue.Length);
        Assert.Equal(x, positionValue[0]);
        Assert.Equal(y, positionValue[1]);

        var visibleProperty = uiModulePropertyObject.GetType().GetProperty("visible");
        Assert.Equal(visible, visibleProperty.GetValue(uiModulePropertyObject, null));

        var scaleProperty = uiModulePropertyObject.GetType().GetProperty("scale");
        Assert.Equal(scale, scaleProperty.GetValue(uiModulePropertyObject, null));

        var positionUpdateProperty = uiModulePropertyObject.GetType().GetProperty("position_update");
        Assert.True(positionUpdateProperty.GetValue(uiModulePropertyObject, null));

        var visibleUpdateProperty = uiModulePropertyObject.GetType().GetProperty("visible_update");
        Assert.True(visibleUpdateProperty.GetValue(uiModulePropertyObject, null));

        var scaleUpdateProperty = uiModulePropertyObject.GetType().GetProperty("scale_update");
        Assert.True(scaleUpdateProperty.GetValue(uiModulePropertyObject, null));

        return Task.CompletedTask;
    }

    [Fact]
    public Task Gets_Ui_Module_Properties_As_Json()
    {
        var uiModuleService = UiModuleServiceMock();
        var uiModuleProperties = uiModuleService.GetUiModulesPropertiesJson(uiModuleService.GetDefaultSettings());

        Assert.IsType<string>(uiModuleProperties);
        JToken.Parse(uiModuleProperties);

        return Task.CompletedTask;
    }

    [Fact]
    public async Task Applies_Ui_Modules_Configuration()
    {
        var uiModuleService = UiModuleServiceMock();
        var uiModuleProperties = uiModuleService.GetUiModulesPropertiesJson(uiModuleService.GetDefaultSettings());

        await uiModuleService.ApplyComponentSettingsAsync(uiModuleService.GetDefaultSettings());

        _server.Remote.Verify(
            remote => remote.TriggerModeScriptEventArrayAsync("Common.UIModules.SetProperties", uiModuleProperties),
            Times.Once
        );
    }

    [Fact]
    public async Task Creates_And_Applies_Ui_Settings_From_Single_Arguments()
    {
        var uiModuleService = UiModuleServiceMock();
        var uiComponentSettings = new GameModeUiComponentSettings("UnitTest", true, 123.0, 123.0, 1.0);
        var uiModuleProperties = uiModuleService.GetUiModulesPropertiesJson([uiComponentSettings]);

        await uiModuleService.ApplyComponentSettingsAsync("UnitTest", true, 123.0, 123.0, 1.0);

        _server.Remote.Verify(
            remote => remote.TriggerModeScriptEventArrayAsync("Common.UIModules.SetProperties", uiModuleProperties),
            Times.Once
        );
    }
}
