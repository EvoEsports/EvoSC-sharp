using EvoSC.Common.Events;
using GbxRemoteNet;

namespace EvoSC.Common.Interfaces;

public interface IEventManager
{
    public void Subscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler);
    public void Unsubscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler);
    public Task Fire(string name, EventArgs args);
}
