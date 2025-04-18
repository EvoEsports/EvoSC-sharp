using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Events;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Events.Args;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Events.Args.Nats;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces.StateMessages;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Settings;
using NATS.Client.JetStream;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Services;

// [Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class NatsBackgroundService(INatsConnectionService nats, INatsSettings natsSettings, IEventManager events)
{

    /*public async Task StartAsync()
    {
        await nats.ConnectAsync();

        var opts = new PushSubscribeOptions.PushSubscribeOptionsBuilder()
            .WithConfiguration(new ConsumerConfiguration.ConsumerConfigurationBuilder()
                .WithDeliverPolicy(DeliverPolicy.Last)
                .WithAckPolicy(AckPolicy.Explicit)
                .Build()
            )
            .Build();
        
        nats.PlayerStateUpdated += NatsOnPlayerStateUpdated;
        _nats.ChatMessageReceived += NatsOnChatMessageReceived;
        _nats.MapFinishedReceived += NatsOnMapFinishedReceived;
    }

    private void NatsOnMapFinishedReceived(object? sender, NatsMessageEventArgs<IStateMessage> e) =>
        _events.RaiseAsync(ServerSyncEvents.ChatMessage, new MapFinishedStateEventArgs());

    private void NatsOnChatMessageReceived(object? sender, NatsMessageEventArgs<IChatStateStateMessage> e) =>
        _events.RaiseAsync(ServerSyncEvents.ChatMessage, new ChatStateMessageEventArgs
        {
            ChatMessage = e.Data ?? throw new InvalidOperationException("Chat message state data is null.")
        });

    private void NatsOnPlayerStateUpdated(object? sender, NatsMessageEventArgs<IPlayerStateUpdateMessage> e) =>
        _events.RaiseAsync(ServerSyncEvents.PlayerStateUpdate, new PlayerStateUpdateEventArgs
        {
            PlayerState = e.Data ?? throw new InvalidOperationException("Player state message data is null.")
        });

    public Task StopAsync()
    {
        _nats.PlayerStateUpdated -= NatsOnPlayerStateUpdated;
        _nats.ChatMessageReceived -= NatsOnChatMessageReceived;
        
        _nats.DisconnectAsync();

        return Task.CompletedTask;
    }*/
}
