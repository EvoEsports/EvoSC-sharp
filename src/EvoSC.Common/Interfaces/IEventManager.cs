using EvoSC.Common.Events;
using GbxRemoteNet;

namespace EvoSC.Common.Interfaces;

public interface IEventManager
{
    public void Subscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler, bool runAsync=false) where TArgs : EventArgs;
    public void Unsubscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler) where TArgs : EventArgs;
    public Task Fire(string name, EventArgs args);
}
