using System.Text.Json;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Settings;
using Microsoft.Extensions.Logging;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class NatsJetstreamService(
    INatsConnectionService natsConnection,
    INatsSettings natsSettings,
    ILogger<NatsJetstreamService> logger) : INatsJetstreamService
{
    public async Task PublishMessageAsync<T>(string subject, T message)
    {
        if (message is null)
        {
            throw new InvalidOperationException("Message cannot be null");
        }
        var fullSubject = $"{natsSettings.MessageGroup}.{subject}";
        var messageJson = JsonSerializer.Serialize<T>(message);
        logger.LogTrace("Publishing message to {Subject} with content {Data}", fullSubject, messageJson);
        PubAckResponse ack =
            await natsConnection.NatsJsContext.PublishAsync(fullSubject, message);

        ack.EnsureSuccess();
    }
}
