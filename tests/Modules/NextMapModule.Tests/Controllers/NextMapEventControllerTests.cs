using EvoSC.Common.Config.Models;
using EvoSC.Common.Config.Models.ThemeOptions;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Themes;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.NextMapModule.Controllers;
using EvoSC.Modules.Official.NextMapModule.Interfaces;
using EvoSC.Testing.Controllers;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.NextMapModule.Tests.Controllers;

public class NextMapEventControllerTests : ControllerMock<NextMapEventController, IEventControllerContext>
{
    private const string Template = "NextMapModule.NextMap";

    private readonly Mock<INextMapService> _nextMapService = new();
    private readonly Mock<IManialinkManager> _manialinkManager = new();
    private readonly Mock<IEvoScBaseConfig> _config = new();
    private readonly Mock<IThemeManager> _themes = new();

    public NextMapEventControllerTests()
    {
        /* var uiTheme = new Mock<IUIThemeConfig>();
        uiTheme.Setup(p => p.HeaderBackgroundColor).Returns("");
        uiTheme.Setup(p => p.PrimaryColor).Returns("fff");
        uiTheme.Setup(p => p.LogoWhiteUrl).Returns("");
        uiTheme.Setup(p => p.PlayerRowBackgroundColor).Returns("111");

        var theme = new Mock<IThemeConfig>();
        theme.Setup(p => p.UI).Returns(uiTheme.Object); */

        var theme = new DynamicThemeOptions(new Dictionary<string, object>
        {
            { "UI.HeaderBackground", "" },
            { "UI.PrimaryColor", "ff" },
            { "UI.LogoWhiteUrl", "" },
            { "UI.PlayerRowBackgroundColor", "111" }
        });
        
        _config.Setup(p => p.Theme).Returns(theme);
        _themes.Setup(p => p.Theme).Returns(theme);
        
        InitMock(_nextMapService, _manialinkManager, _config, _themes);
    }


    [Fact]
    public async Task OnPodiumStart_Shows_Next_Map()
    {
        var map = new DbMap
        {
            AuthorId = 1,
            Enabled = true,
            Id = 123,
            ExternalId = "1337",
            Name = "snippens dream",
            Uid = "Uid"
        };
        _nextMapService.Setup(r => r.GetNextMapAsync()).Returns(Task.FromResult((IMap) map));

        await Controller.ShowNextMapOnPodiumStartAsync(new(), new PodiumEventArgs
        {
            Time = 0
        });
        
        _manialinkManager.Verify(m => m.SendManialinkAsync(Template, It.IsAny<object>()), Times.Once());
    }

    [Fact]
    public async Task OnPodiumEnd_Hides_Next_Map()
    {
        var map = new DbMap
        {
            AuthorId = 1,
            Enabled = true,
            Id = 123,
            ExternalId = "1337",
            Name = "snippens dream",
            Uid = "Uid"
        };
        _nextMapService.Setup(r => r.GetNextMapAsync()).Returns(Task.FromResult((IMap) map));
        
        await Controller.HideNextMapOnPodiumEndAsync(new(), null);
        _manialinkManager.Verify(r => r.HideManialinkAsync(Template), Times.Once);
    }
}
