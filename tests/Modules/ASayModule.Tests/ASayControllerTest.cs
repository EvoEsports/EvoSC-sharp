using EvoSC.Modules.Official.ASayModule.Controllers;
using EvoSC.Modules.Official.ASayModule.Interfaces;
using EvoSC.Testing.Controllers;
using Moq;

namespace EvoSC.Modules.Official.ASayModule.Tests;

public class ASayControllerTest : CommandInteractionControllerTestBase<ASayController>
{
    private readonly ASayController _controller;
    private readonly Mock<IASayService> _service = new();
    
    public ASayControllerTest()
    {
        _controller = new ASayController(_service.Object);
    }

    [Fact]
    private async void Should_Show_Announcement_Message()
    {
        var text = "example message";
        await _controller.ShowAnnounceMessageToPlayersAsync(text);
        _service.Verify(service => service.ShowAnnouncementAsync(text));
    }

    [Fact]
    private async void Should_Clear_Announcement_Message_With_Empty_Param()
    {
        var empty = string.Empty;
        await _controller.ShowAnnounceMessageToPlayersAsync(empty);
        _service.Verify(service => service.HideAnnouncementAsync());
    }

    [Fact]
    private async void Should_Clear_Announcement_Message()
    {
        await _controller.ClearAnnouncementMessageForPlayersAsync();
        _service.Verify(service => service.HideAnnouncementAsync());
    }
}
