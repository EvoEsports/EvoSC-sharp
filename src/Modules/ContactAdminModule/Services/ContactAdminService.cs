using System.Diagnostics.CodeAnalysis;
using System.Text;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Nsgr.ContactAdminModule.Config;
using EvoSC.Modules.Nsgr.ContactAdminModule.Interfaces;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EvoSC.Modules.Nsgr.ContactAdminModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class ContactAdminService(
    IManialinkManager manialinkManager,
    ILogger<ContactAdminService> logger,
    IServerClient client,
    IContactAdminSettings settings
)
    : IContactAdminService
{
    private readonly HttpClient _http = new();
    
    public async Task ShowWidgetAsync()
    {
        await manialinkManager.SendPersistentManialinkAsync("ContactAdminModule.ContactAdminWidget");
        logger.LogDebug("Showing 'Contact Admin' widget");
    }

    public async Task HideWidgetAsync()
    {
        await manialinkManager.HideManialinkAsync("ContactAdminModule.ContactAdminWidget");
        logger.LogDebug("Hiding 'Contact Admin' widget");
    }

    public async Task ContactAdminAsync()
    {
        var serverName = await client.Remote.GetServerNameAsync();

        var message = new { content = "Help requested on server" + serverName };
        var json = JsonConvert.SerializeObject(message);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _http.PostAsync(settings.WebhookURL, data);
        if (response.IsSuccessStatusCode)
        {
            logger.LogDebug("Successfully executed webhook.");
        }
        else
        {
            logger.LogError("Failed to executed Discord webhook.");
        }
    }
}
