using System;
using EvoSC.Core.Domains;
using EvoSC.Modules.ServerConnection;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection;
using GameHost.V3.Module;
using GameHost.V3.Utility;

namespace EvoSC.Core.Plugins
{
    /// <summary>
    ///     Provide an easy module setup for EvoSC plugins
    /// </summary>
    public abstract class PluginModuleBase : HostModule
    {
        private ServerDomain _lastDomain;

        public PluginModuleBase(HostRunnerScope scope) : base(scope)
        {
        }

        protected override void OnInit()
        {
            TrackDomain<ServerDomain>(domain =>
            {
                if (!domain.Scope.Context.TryGet(out IDependencyResolver resolver))
                    throw new InvalidOperationException("Couldn't get resolver");

                var dependencyCollection = new DependencyCollection(domain.Scope.Context, resolver);
                Disposables.Add(dependencyCollection);

                dependencyCollection.Add(new ServerIsConnectedDependency());
                dependencyCollection.OnFinal(() =>
                {
                    _lastDomain = domain;
                    OnServerFound(domain);
                    _lastDomain = null;
                });
            });
        }

        protected abstract void OnServerFound(ServerDomain domain);

        protected void AddSystems(params AppSystem[] systems)
        {
            if (_lastDomain == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(AddSystems)} was called outside of the function OnServerFound"
                );
            }

            Disposables.AddRange(systems as IDisposable[]);
        }
    }
}