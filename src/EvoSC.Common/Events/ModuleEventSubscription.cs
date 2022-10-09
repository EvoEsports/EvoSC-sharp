namespace EvoSC.Common.Events;

public class ModuleEventSubscription : EventSubscription
{
    public Guid ModuleId { get; set; }
    
    public ModuleEventSubscription(AsyncEventHandler<EventArgs> handler, Guid moduleId, bool runAsync=false) : base(handler, runAsync)
    {
        ModuleId = moduleId;
    }
}
