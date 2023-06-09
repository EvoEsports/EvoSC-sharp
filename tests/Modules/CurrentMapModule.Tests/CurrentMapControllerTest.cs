using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.CurrentMapModule.Controllers;
using EvoSC.Modules.Official.CurrentMapModule.Interfaces;
using GbxRemoteNet.Events;
using Moq;

namespace EvoSC.Modules.Official.CurrentMapModule.Tests;

public class CurrentMapControllerTest
{
    private readonly CurrentMapController _controller;
    private readonly Mock<ICurrentMapService> _service = new();

    public CurrentMapControllerTest()
    {
        _controller = new CurrentMapController(_service.Object);
    }

    [Fact]
    private async void Should_On_Begin_Match_Async()
    {
        await _controller.OnBeginMatchAsync(new object(), EventArgs.Empty);

        _service.Verify(service => service.ShowWidgetAsync());
    }

    [Fact]
    private async void Should_On_Begin_Map_Async()
    {
        var args = new MapGbxEventArgs();
        await _controller.OnBeginMapAsync(new object(), args);

        _service.Verify(service => service.ShowWidgetAsync(args));
    }

    [Fact]
    private async void Should_On_Podium_Start_Async()
    {
        var args = new PodiumEventArgs { Time = 0 };
        await _controller.OnPodiumStartAsync(new object(), args);

        _service.Verify(service => service.HideWidgetAsync());
    }
}
