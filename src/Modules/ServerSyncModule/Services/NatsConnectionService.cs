using System.Text;
using System.Text.Json;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Events.Args.Nats;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces.StateMessages;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Models;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Models.StateMessages;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Settings;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Utils;
using Microsoft.Extensions.Logging;
using NATS.Client;
using NATS.Client.JetStream;
using NATS.Client.KeyValue;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class NatsConnectionService(
    INatsSettings natsSettings,
    IServerClient tmServer,
    ILogger<NatsConnectionService> logger)
    : INatsConnectionService
{
    private readonly ConnectionFactory _natsConnFactory = new();

    private List<ISubscription> _subscriptions = new();

    private IConnection? _natsConnection = null;
    private IJetStream? _jetStream = null;
    private IKeyValue? _keyValue = null;

    public IConnection Connection => _natsConnection ?? throw new InvalidOperationException("Not connected to NATS.");

    public IJetStream JetStream =>
        _jetStream ?? throw new InvalidOperationException("The JetStream context has not been created.");

    public IKeyValue KeyValue =>
        _keyValue ?? throw new InvalidOperationException("The KeyValue context has not been created.");

    public string ClientId { get; } = Guid.NewGuid().ToString();

    public async Task ConnectAsync()
    {
        if (_natsConnection != null && !_natsConnection.IsClosed())
        {
            throw new InvalidOperationException("A connection to NATS is already open.");
        }

        var options = ConnectionFactory.GetDefaultOptions();
        options.Url = natsSettings.GetConnectionUrl();
        options.AllowReconnect = true;
        options.MaxReconnect = Options.ReconnectForever;
        options.ReconnectWait = 1000;

        options.Name = ClientId;

        var serverName = await tmServer.Remote.GetServerNameAsync();
        var subjectPrefix = $"{natsSettings.MessageGroup}" + (serverName != null ? $".{serverName}" : "");

        logger.LogDebug("Connecting to {ConnectionUrl} with subjectprefix {SubjectPrefix}...", natsSettings.GetConnectionUrl(),
            subjectPrefix);
        _natsConnection = _natsConnFactory.CreateConnection(options);

        _jetStream = _natsConnection.CreateJetStreamContext();
        _keyValue = _natsConnection.CreateKeyValueContext(natsSettings.KeyVaultBucketName);

        logger.LogDebug("Connected to NATS server at {Url} with stream name {StreamName}", options.Url,
            natsSettings.StreamName);

        try
        {
            _jetStream.GetStreamContext(natsSettings.StreamName);
        }
        catch (NATSTimeoutException)
        {
            logger.LogWarning("No stream exists with the name {StreamName}, attempting to create one.",
                natsSettings.StreamName);
        }


        try
        {
            _subscriptions.Add(JetStream.PushSubscribeAsync(
                    $"{subjectPrefix}.{StateSubjects.ChatMessages.GetIdentifier()}",
                    OnChatMessage,
                    false,
                    new PushSubscribeOptions.PushSubscribeOptionsBuilder()
                        .WithConfiguration(new ConsumerConfiguration.ConsumerConfigurationBuilder()
                            .WithDeliverPolicy(DeliverPolicy.All)
                            .WithAckPolicy(AckPolicy.Explicit)
                            .Build()
                        )
                        .WithStream(natsSettings.StreamName)
                        .Build()
                )
            );
        }
        catch (NATSJetStreamClientException e)
        {
            logger.LogError(e, "Could not subscribe to OnChatMessage with subject {Subject}",
                $"{subjectPrefix}.{StateSubjects.ChatMessages.GetIdentifier()}");
        }

        try
        {
            _subscriptions.Add(_jetStream.PushSubscribeAsync(
                    $"{subjectPrefix}.{StateSubjects.PlayerState.GetIdentifier()}",
                    OnPlayerState,
                    false,
                    new PushSubscribeOptions.PushSubscribeOptionsBuilder()
                        .WithConfiguration(new ConsumerConfiguration.ConsumerConfigurationBuilder()
                            .WithDeliverPolicy(DeliverPolicy.All)
                            .WithStartSequence(natsSettings.PlayerStatesStartSequence)
                            .WithReplayPolicy(ReplayPolicy.Instant)
                            .WithAckPolicy(AckPolicy.Explicit)
                            .Build()
                        )
                        .WithStream(natsSettings.StreamName)
                        .Build()
                )
            );
        }
        catch (NATSJetStreamClientException e)
        {
            logger.LogError(e, "Could not subscribe to OnPlayerState with subject {Subject}",
                $"{subjectPrefix}.{StateSubjects.PlayerState.GetIdentifier()}");
        }

        try
        {
            _subscriptions.Add(_jetStream.PushSubscribeAsync(
                    $"{subjectPrefix}.{StateSubjects.MapFinished.GetIdentifier()}",
                    OnMapFinished,
                    true,
                    new PushSubscribeOptions.PushSubscribeOptionsBuilder()
                        .WithConfiguration(new ConsumerConfiguration.ConsumerConfigurationBuilder()
                            .WithDeliverPolicy(DeliverPolicy.Last)
                            .WithReplayPolicy(ReplayPolicy.Instant)
                            .WithAckPolicy(AckPolicy.Explicit)
                            .Build()
                        )
                        .WithStream(natsSettings.StreamName)
                        .Build()
                )
            );
        }
        catch (NATSJetStreamClientException e)
        {
            logger.LogError(e, "Could not subscribe to OnMapFinished with subject {Subject}",
                $"{subjectPrefix}.{StateSubjects.MapFinished.GetIdentifier()}");
        }
    }

    private void OnMapFinished(object? sender, MsgHandlerEventArgs e)
    {
        if (IsSelf(e.Message))
        {
            e.Message.Ack();
            return;
        }

        e.Message.InProgress();

        try
        {
            var data = e.Message.Deserialize<PlayerStateUpdateMessage>();
            MapFinishedReceived?.Invoke(sender,
                new NatsMessageEventArgs<IStateMessage>
                {
                    Message = e.Message
                });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to process map finished event");
            e.Message.Nak();
            return;
        }

        e.Message.Ack();
    }

    private void OnPlayerState(object? sender, MsgHandlerEventArgs e)
    {
        if (IsSelf(e.Message))
        {
            e.Message.Ack();
            return;
        }

        e.Message.InProgress();

        try
        {
            var data = e.Message.Deserialize<PlayerStateUpdateMessage>();
            PlayerStateUpdated?.Invoke(sender,
                new NatsMessageEventArgs<IPlayerStateUpdateMessage>
                {
                    Message = e.Message, Data = data
                });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to process player state message");
            e.Message.Nak();
            return;
        }

        e.Message.Ack();
    }

    private void OnChatMessage(object? sender, MsgHandlerEventArgs e)
    {
        if (IsSelf(e.Message))
        {
            e.Message.Ack();
            return;
        }

        e.Message.InProgress();

        try
        {
            var data = e.Message.Deserialize<ChatStateStateMessage>();
            ChatMessageReceived?.Invoke(sender,
                new NatsMessageEventArgs<IChatStateStateMessage>
                {
                    Message = e.Message, Data = data
                });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to process chat message");
            e.Message.Nak();
            return;
        }

        e.Message.Ack();
    }

    public async Task DisconnectAsync()
    {
        if (_natsConnection == null || _natsConnection.IsClosed())
        {
            throw new InvalidOperationException("The connection to the NATS server is already closed.");
        }

        await _natsConnection.DrainAsync();
        _natsConnection.Close();
        _natsConnection.Dispose();
    }

    public async Task PublishStateAsync<TStateMsg>(string subject, TStateMsg message) where TStateMsg : IStateMessage
    {
        message.ClientId = ClientId;
        var serverName = await tmServer.Remote.GetServerNameAsync();
        var subjectPrefix = $"{natsSettings.MessageGroup}" + (serverName != null ? $".{serverName}" : "");
        var serialized = JsonSerializer.Serialize(message);
        logger.LogDebug("{SubjectPrefix}.{Subject}", subjectPrefix, subject);
        await JetStream.PublishAsync($"{subjectPrefix}.{subject}", Encoding.UTF8.GetBytes(serialized));
    }

    public Task PublishStateAsync<TStateMsg>(Enum subject, TStateMsg message) where TStateMsg : IStateMessage =>
        PublishStateAsync(subject.GetIdentifier(), message);

    public event EventHandler<NatsMessageEventArgs<IPlayerStateUpdateMessage>>? PlayerStateUpdated;
    public event EventHandler<NatsMessageEventArgs<IChatStateStateMessage>>? ChatMessageReceived;
    public event EventHandler<NatsMessageEventArgs<IStateMessage>>? MapFinishedReceived;

    private bool IsSelf(Msg msg)
    {
        var data = msg.Deserialize<ChatStateStateMessage>();
        return data != null && data.ClientId.Equals(ClientId, StringComparison.Ordinal);
    }
}