using NATS.Client.JetStream;
using NATS.Client.KeyValueStore;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;

public interface INatsConnectionService
{
    /// <summary>
    ///  The NATS JetStream context.
    /// </summary>
    INatsJSContext NatsJsContext { get; }
    
    /// <summary>
    /// The NATS Key Value store.
    /// </summary>
    INatsKVStore NatsKvStore { get; }
    
    /// <summary>
    /// The NATS JetStream consumer.
    /// </summary>
    INatsJSConsumer NatsJsConsumer { get; }
    
    /// <summary>
    /// Establish a connection to NATS.
    /// </summary>
    /// <returns></returns>
    Task ConnectAsync();
}
