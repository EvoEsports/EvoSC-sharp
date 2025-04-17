using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;
using Microsoft.Extensions.Logging;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Services;

public class NatsJetstreamService(INatsConnectionService natsConnection, ILogger<NatsJetstreamService> logger)
{
    public async Task PublishMessageAsync<T>(string subject, T message)
    {
        PubAckResponse ack = await natsConnection.NatsJsContext.PublishAsync(subject, message);

        if (ack.Error != null)
        {
            logger.LogWarning("Failed to publish message to subject {Subject}: {AckError}", subject,
                ack.Error.Description);
        }

        ack.EnsureSuccess();
    }

    public async Task FetchMessageAsync<T>(string subject, CancellationToken cancellationToken, int? maxBytes,
        int maxMsgs = 1000)
    {
        await foreach (NatsJSMsg<T> msg in natsConnection.NatsJsConsumer
                           .FetchAsync<T>(new NatsJSFetchOpts { MaxMsgs = maxMsgs, MaxBytes = maxBytes })
                           .WithCancellation(cancellationToken))
        {
        }
    }

    public async Task ConsumeMessageAsync<T>(string subject, CancellationToken cancellationToken)
    {
        await foreach (NatsJSMsg<T> msg in natsConnection.NatsJsConsumer.ConsumeAsync<T>()
                           .WithCancellation(cancellationToken))
        {
            msg.Data
            await msg.AckAsync(cancellationToken: cancellationToken);
        }
    }
}
