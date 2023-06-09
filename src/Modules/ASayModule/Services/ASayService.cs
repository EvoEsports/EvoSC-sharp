using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.ASayModule.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.ASayModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class ASayService : IASayService
{
    private readonly ILogger<ASayService> _logger;
    private readonly IManialinkManager _manialinkManager;

    public ASayService(ILogger<ASayService> logger, IManialinkManager manialinkManager)
    {
        _logger = logger;
        _manialinkManager = manialinkManager;
    }

    public async Task OnDisable()
    {
        await _manialinkManager.HideManialinkAsync("ASayModule.Announcement");
        _logger.LogInformation("ASayModule disabled.");
    }

    public async Task ShowAnnouncement(string text)
    {
        await _manialinkManager.SendManialinkAsync("ASayModule.Announcement", new {text});
    }

    public async Task HideAnnouncement()
    {
        await _manialinkManager.HideManialinkAsync("ASayModule.Announcement");
    }
}
