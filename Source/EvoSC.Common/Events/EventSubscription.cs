namespace EvoSC.Common.Events;

public class EventSubscription
{
    public AsyncEventHandler Handler { get; init; }
    public bool RunAsync { get; set; }

    public EventSubscription(AsyncEventHandler handler, bool runAsync=false)
    {
        Handler = handler;
    }
}
