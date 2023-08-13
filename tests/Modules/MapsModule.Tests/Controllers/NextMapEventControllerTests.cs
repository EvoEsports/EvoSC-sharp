using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.Maps.Controllers;
using EvoSC.Modules.Official.Maps.Interfaces;
using EvoSC.Testing.Controllers;
using Moq;

namespace EvoSC.Modules.Official.MapsModule.Tests.Controllers;

public class NextMapEventControllerTests : ControllerMock<NextMapEventController, IEventControllerContext>
{
    private const string Template = "Maps.NextMap";

    private readonly Mock<INextMapService> _nextMapService = new();
    private readonly Mock<IManialinkManager> _manialinkManager = new();

    public NextMapEventControllerTests()
    {
        InitMock(_nextMapService, _manialinkManager);
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

        await Controller.ShowNextMapOnPodiumStartAsync(new(), null);
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
