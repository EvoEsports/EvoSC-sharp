using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Settings;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Utils;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using NATS.Client.KeyValueStore;
using NATS.Net;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Services;

public class NatsConnectionService(NatsClient? natsClient, INatsSettings natsSettings)
    : INatsConnectionService, IAsyncDisposable
{
    private NatsClient? _natsClient = natsClient;

    private INatsJSContext? _natsJsContext = null;
    private INatsKVStore? _natsKVStore = null;
    private INatsJSConsumer? _natsJSConsumer = null;

    public INatsJSContext NatsJsContext => _natsJsContext ?? throw new InvalidOperationException(
        "NATS JetStream context is not initialized."
    );

    public INatsKVStore NatsKvStore => _natsKVStore ?? throw new InvalidOperationException(
        "NATS Key Value context is not initialized."
    );
    
    public INatsJSConsumer NatsJsConsumer => _natsJSConsumer ?? throw new InvalidOperationException(
        "NATS JetStream consumer is not initialized."
    );

    public async Task ConnectAsync()
    {
        if (_natsClient is not null)
        {
            return;
        }

        _natsClient = new NatsClient(natsSettings.GetConnectionUrl());

        var jsContext = _natsClient.CreateJetStreamContext();

        await jsContext.CreateOrUpdateStreamAsync(new StreamConfig(
            name: natsSettings.StreamName,
            subjects: [$"{natsSettings.MessageGroup}.>"]
        ));
        _natsJSConsumer = await jsContext.CreateOrUpdateConsumerAsync(natsSettings.StreamName,
            new ConsumerConfig(name: natsSettings.ConsumerName));

        if (natsSettings.UseKeyValueStore)
        {
            var kvStoreContext = _natsClient.CreateKeyValueStoreContext();
            _natsKVStore =
                await kvStoreContext.CreateOrUpdateStoreAsync(new NatsKVConfig(natsSettings.KeyVaultBucketName));
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_natsClient != null)
        {
            await _natsClient.DisposeAsync();
        }
    }
}
