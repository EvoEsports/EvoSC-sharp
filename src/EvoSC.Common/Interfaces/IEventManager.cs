using EvoSC.Common.Events;
using EvoSC.Common.Interfaces.Controllers;
using GbxRemoteNet;

namespace EvoSC.Common.Interfaces;

public interface IEventManager : IControllerActionRegistry
{
    public void Subscribe(EventSubscription subscription);
    public void Subscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler, EventPriority priority=EventPriority.Medium, bool runAsync=false) where TArgs : EventArgs;
    public void Unsubscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler) where TArgs : EventArgs;
    public Task Fire(string name, EventArgs args, object? sender=null);
}
