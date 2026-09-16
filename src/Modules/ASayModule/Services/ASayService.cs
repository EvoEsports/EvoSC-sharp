using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.ASayModule.Events;
using EvoSC.Modules.Official.ASayModule.Interfaces;

namespace EvoSC.Modules.Official.ASayModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class ASayService(IManialinkManager manialinkManager)
    : IASayService
{
    public async Task ShowAnnouncementAsync(string text)
    {
        await manialinkManager.SendPersistentManialinkAsync("ASayModule.Announcement", new {text});
    }

    public async Task HideAnnouncementAsync()
    {
        await manialinkManager.HideManialinkAsync("ASayModule.Announcement");
    }
}
