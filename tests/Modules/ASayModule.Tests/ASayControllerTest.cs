using EvoSC.Modules.Official.ASayModule.Controllers;
using EvoSC.Modules.Official.ASayModule.Interfaces;
using EvoSC.Testing.Controllers;
using NSubstitute;

namespace EvoSC.Modules.Official.ASayModule.Tests;

public class ASayControllerTest : CommandInteractionControllerTestBase<ASayController>
{
    private readonly ASayController _controller;
    private readonly IASayService _service = Substitute.For<IASayService>();
    
    public ASayControllerTest()
    {
        _controller = new ASayController(_service);
    }

    [Fact]
    private async void Should_Show_Announcement_Message()
    {
        var text = "example message";
        await _controller.ShowAnnounceMessageToPlayersAsync(text);
        await _service.Received().ShowAnnouncementAsync(text);
    }

    [Fact]
    private async void Should_Clear_Announcement_Message_With_Empty_Param()
    {
        var empty = string.Empty;
        await _controller.ShowAnnounceMessageToPlayersAsync(empty);
        await _service.Received().HideAnnouncementAsync();
    }

    [Fact]
    private async void Should_Clear_Announcement_Message()
    {
        await _controller.ClearAnnouncementMessageForPlayersAsync();
        await _service.Received().HideAnnouncementAsync();
    }
}
