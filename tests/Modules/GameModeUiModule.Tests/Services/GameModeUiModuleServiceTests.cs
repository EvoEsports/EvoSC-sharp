using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Official.GameModeUiModule.Config;
using EvoSC.Modules.Official.GameModeUiModule.Interfaces;
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
    public async Task Generates_Property_Object(string id, bool visible, double x, double y, double scale)
    {
        var uiModuleService = UiModuleServiceMock();
        var uiModulePropertyObject = await uiModuleService.GeneratePropertyObjectAsync(id, visible, x, y, scale);

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
    }

    [Fact]
    public async Task Gets_Ui_Module_Properties_As_Json()
    {
        var uiModuleService = UiModuleServiceMock();
        var uiModuleProperties = await uiModuleService.GetUiModulesPropertiesJsonAsync();

        Assert.IsType<string>(uiModuleProperties);
        JToken.Parse(uiModuleProperties);
    }

    [Fact]
    public async Task Applies_Ui_Modules_Configuration()
    {
        var uiModuleService = UiModuleServiceMock();
        var uiModuleProperties = await uiModuleService.GetUiModulesPropertiesJsonAsync();

        await uiModuleService.ApplyConfigurationAsync();

        _server.Remote.Verify(
            remote => remote.TriggerModeScriptEventArrayAsync("Common.UIModules.SetProperties", uiModuleProperties),
            Times.Once
        );
    }
}
