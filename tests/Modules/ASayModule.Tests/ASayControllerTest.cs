using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.ASayModule.Controllers;
using EvoSC.Modules.Official.ASayModule.Interfaces;
using EvoSC.Testing.Controllers;
using Moq;

namespace EvoSC.Modules.Official.ASayModule.Tests;

public class ASayControllerTest : CommandInteractionControllerTestBase<ASayController>
{
    private readonly Mock<IOnlinePlayer> _actor = new();
    private readonly Mock<IASayService> _service = new();

    public ASayControllerTest()
    {
        InitMock(_actor.Object, _service);
    }

    [Fact]
    private async void Should_Show_Announcement_Message()
    {
        var text = "example message";
        await Controller.ShowAnnounceMessageToPlayersAsync(text);
        _service.Verify(service => service.ShowAnnouncementAsync(text));
    }

    [Fact]
    private async void Should_Clear_Announcement_Message_With_Empty_Param()
    {
        var empty = string.Empty;
        await Controller.ShowAnnounceMessageToPlayersAsync(empty);
        _service.Verify(service => service.HideAnnouncementAsync());
    }

    [Fact]
    private async void Should_Clear_Announcement_Message()
    {
        await Controller.ClearAnnouncementMessageForPlayersAsync();
        _service.Verify(service => service.HideAnnouncementAsync());
    }
}
