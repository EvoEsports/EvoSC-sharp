using EvoSC.Modules.Official.ASayModule.Controllers;
using EvoSC.Modules.Official.ASayModule.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EvoSC.Modules.Official.ASayModule.Tests;

public class ASayControllerTest
{
    private readonly ASayController _controller;
    private readonly ILogger<ASayController> _logger = new NullLogger<ASayController>();
    private readonly Mock<IASayService> _service = new();
    
    public ASayControllerTest()
    {
        _controller = new ASayController(_logger, _service.Object);
    }

    [Fact]
    private async void Should_Show_Announcement_Message()
    {
        var text = "example message";
        await _controller.ShowAnnounceMessageToPlayers(text);
        _service.Verify(service => service.ShowAnnouncement(text));
    }

    [Fact]
    private async void Should_Clear_Announcement_Message_With_Empty_Param()
    {
        var empty = string.Empty;
        await _controller.ShowAnnounceMessageToPlayers(empty);
        _service.Verify(service => service.HideAnnouncement());
    }

    [Fact]
    private async void Should_Clear_Announcement_Message()
    {
        await _controller.ClearAnnouncementMessageToPlayers();
        _service.Verify(service => service.HideAnnouncement());
    }
}
