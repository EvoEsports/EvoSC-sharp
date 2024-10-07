using EvoSC.Modules.EvoEsports.ServerSyncModule.Events.Args.Nats;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces.StateMessages;
using NATS.Client;
using NATS.Client.JetStream;
using NATS.Client.KeyValue;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;

public interface INatsConnectionService
{
    /// <summary>
    /// Connection to the NATS server.
    /// </summary>
    public IConnection Connection { get; }
    
    /// <summary>
    /// The stream context.
    /// </summary>
    public IJetStream JetStream { get; }
    
    /// <summary>
    /// The key value context.
    /// </summary>
    public IKeyValue KeyValue { get; }
    
    /// <summary>
    /// The ID/Name of the current client.
    /// </summary>
    public string ClientId { get; }
    
    /// <summary>
    /// Establish a connection to NATS.
    /// </summary>
    /// <returns></returns>
    internal Task ConnectAsync();
    
    /// <summary>
    /// Disconnect from NATS.
    /// </summary>
    /// <returns></returns>
    internal Task DisconnectAsync();

    /// <summary>
    /// Publish a state message to the NATS stream.
    /// </summary>
    /// <param name="subject">The subject to publish to.</param>
    /// <param name="message">The message to publish.</param>
    /// <typeparam name="TStateMsg"></typeparam>
    /// <returns></returns>
    public Task PublishStateAsync<TStateMsg>(string subject, TStateMsg message) where TStateMsg : IStateMessage;
    
    /// <summary>
    /// Publish a state message to the NATS stream.
    /// </summary>
    /// <param name="subject">The subject to publish to.</param>
    /// <param name="message">The message to publish.</param>
    /// <typeparam name="TStateMsg"></typeparam>
    /// <returns></returns>
    public Task PublishStateAsync<TStateMsg>(Enum subject, TStateMsg message) where TStateMsg : IStateMessage;

    /// <summary>
    /// When a player state update message has been received.
    /// </summary>
    public event EventHandler<NatsMessageEventArgs<IPlayerStateUpdateMessage>> PlayerStateUpdated;
    
    /// <summary>
    /// When a chat message has been received.
    /// </summary>
    public event EventHandler<NatsMessageEventArgs<IChatStateStateMessage>> ChatMessageReceived;

    public event EventHandler<NatsMessageEventArgs<IStateMessage>> MapFinishedReceived;
}
