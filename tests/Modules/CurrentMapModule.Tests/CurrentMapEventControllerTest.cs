using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.CurrentMapModule.Controllers;
using EvoSC.Modules.Official.CurrentMapModule.Interfaces;
using GbxRemoteNet.Events;
using Moq;

namespace EvoSC.Modules.Official.CurrentMapModule.Tests;

public class CurrentMapEventControllerTest
{
    private readonly CurrentMapEventController _eventController;
    private readonly Mock<ICurrentMapService> _service = new();

    public CurrentMapEventControllerTest()
    {
        _eventController = new CurrentMapEventController(_service.Object);
    }

    [Fact]
    private async Task Should_On_Begin_Match_Async()
    {
        await _eventController.OnBeginMatchAsync(new object(), EventArgs.Empty);

        _service.Verify(service => service.ShowWidgetAsync());
    }

    [Fact]
    private async Task Should_On_Begin_Map_Async()
    {
        var args = new MapGbxEventArgs();
        await _eventController.OnBeginMapAsync(new object(), args);

        _service.Verify(service => service.ShowWidgetAsync(args));
    }

    [Fact]
    private async Task Should_On_Podium_Start_Async()
    {
        var args = new PodiumEventArgs { Time = 0 };
        await _eventController.OnPodiumStartAsync(new object(), args);

        _service.Verify(service => service.HideWidgetAsync());
    }
}
