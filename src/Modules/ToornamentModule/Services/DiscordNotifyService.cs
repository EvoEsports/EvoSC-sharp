using System.Text;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;
using EvoSC.Modules.EvoEsports.ToornamentModule.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class DiscordNotifyService(
    ILogger<DiscordNotifyService> logger,
    IToornamentSettings settings
    ) : IDiscordNotifyService
{
    private readonly HttpClient _http = new();

    public async Task NotifyMatchInfoAsync(string matchName, List<IMap> maps)
    {
        if (string.IsNullOrEmpty(settings.WebhookUrl))
        {
            logger.LogDebug("Discord webhook url is not setup, so no matchinfo will be shared");
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine($"The map order for match {matchName} is: ");
        sb.AppendLine();
        foreach (var map in maps)
        {
            sb.AppendLine(map.Name);
        }

        // Check for suffix so we can ping people on Discord
        if (!string.IsNullOrEmpty(settings.MessageSuffix))
        {
            sb.AppendLine();
            sb.AppendLine(settings.MessageSuffix);
        }

        var messageObject = new { content = sb.ToString() };
        var json = JsonConvert.SerializeObject(messageObject);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        logger.LogTrace($"Requesting {settings.WebhookUrl}");

        try
        {
            var response = await _http.PostAsync(settings.WebhookUrl, data);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Failed to send message to Discord webhook url with matchinfo");
            }

            logger.LogDebug("Successfully executed webhook.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to execute Discord webhook");
        }
    }
}
