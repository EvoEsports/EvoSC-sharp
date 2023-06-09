using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.ASayModule.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EvoSC.Modules.Official.ASayModule.Tests;

public class ASayServiceTest
{
    private readonly ASayService _aSayService;
    private readonly ILogger<ASayService> _logger = new NullLogger<ASayService>();
    private readonly Mock<IManialinkManager> _manialinkManager = new();

    public ASayServiceTest()
    {
        _aSayService = new ASayService(_logger, _manialinkManager.Object);
    }

    [Fact]
    private async void Should_On_Disable()
    {
        await _aSayService.OnDisableAsync();
        _manialinkManager.Verify(manager => manager.HideManialinkAsync("ASayModule.Announcement"));
    }
    
    [Fact]
    private async void Should_Show_Announcement_Message()
    {
        var text = "example message";
        await _aSayService.ShowAnnouncementAsync(text);
        _manialinkManager.Verify(manager => manager.SendManialinkAsync("ASayModule.Announcement", It.Is<object>(o => text.Equals(o.GetType().GetProperty("text")!.GetValue(o)))));
    }

    [Fact]
    private async void Should_Hide_Announcement_Message()
    {
        await _aSayService.HideAnnouncementAsync();
        _manialinkManager.Verify(manager => manager.HideManialinkAsync("ASayModule.Announcement"));
    }
}
