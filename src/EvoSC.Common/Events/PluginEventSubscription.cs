namespace EvoSC.Common.Events;

public class PluginEventSubscription : EventSubscription
{
    public Guid PluginId { get; set; }
    
    public PluginEventSubscription(AsyncEventHandler<EventArgs> handler, Guid pluginId, bool runAsync=false) : base(handler, runAsync)
    {
        PluginId = pluginId;
    }
}
