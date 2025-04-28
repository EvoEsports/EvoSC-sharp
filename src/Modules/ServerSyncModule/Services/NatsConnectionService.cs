using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Settings;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Utils;
using Microsoft.Extensions.Logging;
using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using NATS.Client.KeyValueStore;
using NATS.Net;
using NATS.NKeys;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class NatsConnectionService(INatsSettings natsSettings, ILogger<INatsConnectionService> logger)
    : INatsConnectionService, IAsyncDisposable
{

    private INatsJSContext? _natsJsContext;
    private INatsKVStore? _natsKvStore;
    private INatsJSConsumer? _natsJsConsumer;
    private NatsClient? _natsClient;

    public INatsJSContext NatsJsContext => _natsJsContext ?? throw new InvalidOperationException(
        "NATS JetStream context is not initialized."
    );

    public INatsKVStore NatsKvStore => _natsKvStore ?? throw new InvalidOperationException(
        "NATS Key Value context is not initialized."
    );
    
    public INatsJSConsumer NatsJsConsumer => _natsJsConsumer ?? throw new InvalidOperationException(
        "NATS JetStream consumer is not initialized."
    );

    public async Task ConnectAsync()
    {
        if (_natsClient is not null)
        {
            logger.LogInformation("NATS connection is already initialized.");
            return;
        }

        if (string.IsNullOrEmpty(natsSettings.NKey))
        {
            _natsClient = new NatsClient(natsSettings.GetConnectionUrl());
        }
        else
        {
            KeyPair keyPair = KeyPair.FromSeed(natsSettings.NKeySeed);
            
            _natsClient = new NatsClient(new NatsOpts {
                Url = natsSettings.GetConnectionUrl(),
                AuthOpts = new NatsAuthOpts
                {
                    NKey = natsSettings.NKey,
                    Seed = keyPair.GetSeed()
                }
            });
        }

        logger.LogDebug("Connecting to NATS server at {Url}", natsSettings.GetConnectionUrl());
        _natsJsContext = _natsClient.CreateJetStreamContext();

        await _natsJsContext.CreateOrUpdateStreamAsync(new StreamConfig(
            name: natsSettings.StreamName,
            subjects: [$"{natsSettings.MessageGroup}.>"]
        ));
        _natsJsConsumer = await _natsJsContext.CreateOrUpdateConsumerAsync(natsSettings.StreamName,
            new ConsumerConfig(name: natsSettings.ConsumerName));

        if (natsSettings.UseKeyValueStore)
        {
            logger.LogDebug("Creating NATS Key Value store with bucket name {BucketName}",
                natsSettings.KeyVaultBucketName);
            var kvStoreContext = _natsClient.CreateKeyValueStoreContext();
            _natsKvStore =
                await kvStoreContext.CreateOrUpdateStoreAsync(new NatsKVConfig(natsSettings.KeyVaultBucketName));
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_natsClient != null)
        {
            logger.LogDebug("Disposing NATS connection.");
            await _natsClient.DisposeAsync();
        }
    }
}
