using System;
using GameHost.V3.Injection;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.IO.Events;

namespace EvoSC.Modules.ServerConnection
{
    public enum ServerConnectionState
    {
        Disconnected,
        Connecting,
        Connected
    }

// used in scopes
//
// (a system may use this as a dependency, then subscribe to it)
// eg:
//  MySystem(Context ctx) {
//      Dependencies.AddRef(() => ref connectionState);
// }
//
//  void OnInit() {
//      Disposable.Add(connectionState.Subscribe((_, state) => {
//          // connection state
//      }, true));
//  }
//      |  ^ setting the last parameter (invokeNow) to true will call your action right now (even if there was no
//      |                                                                                    update)
//      ^ add to the disposable since we want to stop listening when the system get
//        destroyed
//
    public sealed class ServerConnectionStateBindable : ManagedReadOnlyBindable<ServerConnectionState>
    {
        public ServerConnectionStateBindable(Bindable<ServerConnectionState> source) : base(source)
        {
        }
    }

    public class ServerIsConnectedDependency : IDependency, IResolvedObject
    {
        public void Resolve<TContext>(TContext context) where TContext : IReadOnlyContext
        {
            if (!context.TryGet(out ServerConnectionStateBindable bindable))
                return;

            if (bindable.Value == ServerConnectionState.Connected)
                IsResolved = true;
        }

        public Exception ResolveException { get; set; }
        public bool IsResolved { get; private set; }
        public object Resolved => null;
    }
}
