using System;
using System.Text.Json;
using DefaultEcs;
using EvoSC.Core.Configuration;
using EvoSC.Core.Domains;
using GameHost.V3;
using GameHost.V3.Injection;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.IO.Storage;
using GameHost.V3.Module;
using GameHost.V3.Threading.Components;
using GameHost.V3.Threading.V2;
using GameHost.V3.Utility;
using NLog;
using EvoSC.ServerConnection;

namespace EvoSC.ServerConnection
{
    public class Module : HostModule
    {
        // we need the host scope for the server domain
        private readonly HostRunnerScope _hostScope;
        private ILogger _logger;

        private World _world;

        public Module(HostRunnerScope scope) : base(scope)
        {
            _hostScope = scope;

            Dependencies.AddRef(() => ref _world);
            Dependencies.AddRef(() => ref _logger);
        }

        private ServerConnectionConfig GetConfiguration()
        {
            using var pooledFiles = ModuleScope.DataStorage.GetPooledFiles("server.json");
            if (pooledFiles.Count == 0)
            {
                _logger.Fatal($"File 'server.json' not found in {ModuleScope.DataStorage.CurrentPath}");
                return default;
            }

            using var pooledBytes = pooledFiles[0].GetPooledBytes();
            return JsonSerializer.Deserialize<ServerConnectionConfig>(pooledBytes.Span);
        }

        protected override void OnInit()
        {
            // Create the server domain (with a listener on the main thread)
            //
            // Perhaps it can be a good idea to put the domain on another thread?
            // (it can be useful if there would be a need to have multiple domains on the same app)
            //
            // guerro opinion: see whether or not the ServerDomain will interact a lot with the ExecutiveDomain
            //                 from a plugin developer perspective
            //                 if it doesn't then we can safely move it to another thread
            {
                var listenerCollection = _world.CreateEntity();
                listenerCollection.Set<ListenerCollectionBase>(new ListenerCollection());

                var domain = _world.CreateEntity();
                domain.Set(GetConfiguration());
                domain.Set<IListener>(new ServerDomain(_hostScope, domain));
                domain.Set(new PushToListenerCollection(listenerCollection));
            }

            TrackDomain<ServerDomain>(domain =>
            {
                Disposables.AddRange(new IDisposable[]
                {
                    new ConnectToServerSystem(domain.Scope), 
                    new ManageEventLoopSystem(domain.Scope),
                    new CreateEventServerSystem(domain.Scope),
                    
                    new PlayerSystem(domain.Scope),
                    new CreatePlayerOnServerStart(domain.Scope)
                });
            });
        }

        // This module will use the Config folder, or else the config stuff will be in
        // a folder called `EvoSC.Core.ServerConnection/ServerConnection`, kinda bad
        protected override IStorage CreateDataStorage(Scope scope)
        {
            if (!scope.Context.TryGet(out IStorage executingStorage))
            {
                throw new NullReferenceException(nameof(IStorage));
            }

            return executingStorage.GetSubStorage("Config");
        }
    }
}
