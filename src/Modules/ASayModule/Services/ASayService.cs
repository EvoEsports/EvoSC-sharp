﻿using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.ASayModule.Events;
using EvoSC.Modules.Official.ASayModule.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.ASayModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class ASayService : IASayService
{
    private readonly ILogger<ASayService> _logger;
    private readonly IManialinkManager _manialinkManager;
    private readonly IContextService _contextService;
    private readonly dynamic _locale;

    public ASayService(ILogger<ASayService> logger, IManialinkManager manialinkManager, IContextService context, Locale locale)
    {
        _logger = logger;
        _manialinkManager = manialinkManager;
        _contextService = context;
        _locale = locale;
    }

    public async Task OnDisableAsync()
    {
        await _manialinkManager.HideManialinkAsync("ASayModule.Announcement");
        _logger.LogInformation("ASayModule disabled");
        _contextService.Audit().Success()
            .WithEventName(AuditEvents.DisableModule)
            .Comment("ASay Module was disabled.");
    }

    public async Task ShowAnnouncementAsync(string text)
    {
        await _manialinkManager.SendManialinkAsync("ASayModule.Announcement", new {text});
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
