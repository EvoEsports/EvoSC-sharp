using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.ASayModule.Events;
using EvoSC.Modules.Official.ASayModule.Interfaces;

namespace EvoSC.Modules.Official.ASayModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Scoped)]
public class ASayService(IManialinkManager manialinkManager, IContextService context)
    : IASayService
{
    public async Task ShowAnnouncementAsync(string text)
    {
        await manialinkManager.SendPersistentManialinkAsync("ASayModule.Announcement", new {text});
        context.Audit().Success()
            .WithEventName(AuditEvents.ShowAnnouncement)
            .HavingProperties(new {Text = text})
            .Comment("Announcement was shown.");
    }

    public async Task HideAnnouncementAsync()
    {
        await manialinkManager.HideManialinkAsync("ASayModule.Announcement");
        context.Audit().Success()
            .WithEventName(AuditEvents.ClearAnnouncement)
            .Comment("Announcement was cleared.");
    }
}
