using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.ASayModule.Events;
using EvoSC.Modules.Official.ASayModule.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.ASayModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Scoped)]
public class ASayService : IASayService
{
    private readonly IManialinkManager _manialinkManager;
    private readonly IContextService _contextService;

    public ASayService(IManialinkManager manialinkManager, IContextService context)
    {
        _manialinkManager = manialinkManager;
        _contextService = context;
    }

    public async Task ShowAnnouncementAsync(string text)
    {
        await _manialinkManager.SendPersistentManialinkAsync("ASayModule.Announcement", new {text});
        _contextService.Audit().Success()
            .WithEventName(AuditEvents.ShowAnnouncement)
            .HavingProperties(new {Text = text})
            .Comment("Announcement was shown.");
    }

    public async Task HideAnnouncementAsync()
    {
        await _manialinkManager.HideManialinkAsync("ASayModule.Announcement");
        _contextService.Audit().Success()
            .WithEventName(AuditEvents.ClearAnnouncement)
            .Comment("Announcement was cleared.");
    }
}
