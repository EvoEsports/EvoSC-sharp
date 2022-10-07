namespace EvoSC.Common.Events;

public class EventSubscription
{
    public AsyncEventHandler<EventArgs> Handler { get; init; }
    public bool RunAsync { get; set; }

    public EventSubscription(AsyncEventHandler<EventArgs> handler, bool runAsync=false)
    {
        Handler = handler;
    }
}
