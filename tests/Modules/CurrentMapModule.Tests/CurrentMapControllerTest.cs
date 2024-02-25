using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.CurrentMapModule.Controllers;
using EvoSC.Modules.Official.CurrentMapModule.Interfaces;
using GbxRemoteNet.Events;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace EvoSC.Modules.Official.CurrentMapModule.Tests;

public class CurrentMapControllerTest
{
    private readonly CurrentMapController _controller;
    private readonly ICurrentMapService _service = Substitute.For<ICurrentMapService>();

    public CurrentMapControllerTest()
    {
        _controller = new CurrentMapController(_service);
    }

    [Fact]
    private async Task Should_On_Begin_Match_Async()
    {
        await _controller.OnBeginMatchAsync(new object(), EventArgs.Empty);

        await _service.Received().ShowWidgetAsync();
    }

    [Fact]
    private async Task Should_On_Begin_Map_Async()
    {
        var args = new MapGbxEventArgs();
        await _controller.OnBeginMapAsync(new object(), args);

        await _service.Received().ShowWidgetAsync(args);
    }

    [Fact]
    private async Task Should_On_Podium_Start_Async()
    {
        var args = new PodiumEventArgs { Time = 0 };
        await _controller.OnPodiumStartAsync(new object(), args);

        await _service.Received().HideWidgetAsync();
    }
}
