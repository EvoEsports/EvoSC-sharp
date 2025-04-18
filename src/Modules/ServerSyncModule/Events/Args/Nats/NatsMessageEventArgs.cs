using NATS.Client;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Events.Args.Nats;

public class NatsMessageEventArgs : EventArgs
{
    public required Msg Message { get; init; }
}

public class NatsMessageEventArgs<T> : NatsMessageEventArgs
{
    public T? Data { get; init; }
}
