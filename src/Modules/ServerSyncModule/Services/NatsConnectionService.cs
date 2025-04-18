using Castle.Core.Logging;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Settings;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Utils;
using Microsoft.Extensions.Logging;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using NATS.Client.KeyValueStore;
using NATS.Net;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class NatsConnectionService(INatsSettings natsSettings, ILogger<INatsConnectionService> logger)
    : INatsConnectionService, IAsyncDisposable
{

    private INatsJSContext? natsJsContext = null;
    private INatsKVStore? natsKVStore = null;
    private INatsJSConsumer? natsJSConsumer = null;
    private NatsClient? natsClient = null;

    public INatsJSContext NatsJsContext => natsJsContext ?? throw new InvalidOperationException(
        "NATS JetStream context is not initialized."
    );

    public INatsKVStore NatsKvStore => natsKVStore ?? throw new InvalidOperationException(
        "NATS Key Value context is not initialized."
    );
    
    public INatsJSConsumer NatsJsConsumer => natsJSConsumer ?? throw new InvalidOperationException(
        "NATS JetStream consumer is not initialized."
    );

    public async Task ConnectAsync()
    {
        if (natsClient is not null)
        {
            logger.LogInformation("NATS connection is already initialized.");
            return;
        }

        natsClient = new NatsClient(natsSettings.GetConnectionUrl());

        logger.LogDebug("Connecting to NATS server at {Url}", natsSettings.GetConnectionUrl());
        natsJsContext = natsClient.CreateJetStreamContext();

        await natsJsContext.CreateOrUpdateStreamAsync(new StreamConfig(
            name: natsSettings.StreamName,
            subjects: [$"{natsSettings.MessageGroup}.>"]
        ));
        natsJSConsumer = await natsJsContext.CreateOrUpdateConsumerAsync(natsSettings.StreamName,
            new ConsumerConfig(name: natsSettings.ConsumerName));

        if (natsSettings.UseKeyValueStore)
        {
            logger.LogDebug("Creating NATS Key Value store with bucket name {BucketName}",
                natsSettings.KeyVaultBucketName);
            var kvStoreContext = natsClient.CreateKeyValueStoreContext();
            natsKVStore =
                await kvStoreContext.CreateOrUpdateStoreAsync(new NatsKVConfig(natsSettings.KeyVaultBucketName));
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (natsClient != null)
        {
            logger.LogDebug("Disposing NATS connection.");
            await natsClient.DisposeAsync();
        }
    }
}
