using System;
using System.Diagnostics;
using DefaultEcs;
using EvoSC.Core.Configuration;
using EvoSC.Modules.ServerConnection;
using GameHost.V3;
using GameHost.V3.Domains;
using GameHost.V3.Loop;
using GameHost.V3.Loop.EventSubscriber;
using GameHost.V3.Threading.V2.Apps;
using GbxRemoteNet;

namespace EvoSC.Core.Domains
{
    /// <summary>
    /// A domain which maintain a <see cref="GbxRemoteClient"/> connection to a Mania based game.
    /// </summary>
    public class ServerDomain : CommonDomainThreadListener
    {
        private readonly DefaultDomainUpdateLoopSubscriber _updateLoop;
        private readonly DomainWorker _worker;

        public readonly ServerConnectionConfig ConnectionConfig;

        public readonly ServerScope Scope;
        public readonly TimeSpan TargetDelta = TimeSpan.FromMilliseconds(50);

        public readonly IDomainUpdateLoopSubscriber UpdateLoop;

        public readonly IReadOnlyDomainWorker Worker;

        public ServerDomain(HostRunnerScope hostScope, Entity domainEntity) : base(hostScope, domainEntity)
        {
            Debug.Assert(domainEntity.Has<ServerConnectionConfig>(), "domainEntity.Has<ServerConnectionConfig>()");

            ConnectionConfig = domainEntity.Get<ServerConnectionConfig>();
            
            var world = new World();
            Scope = new ServerScope(
                DomainScope,
                world,
                ConnectionConfig
            );
            
            var workerName = $"Server {ConnectionConfig.Host}:{ConnectionConfig.Port}";
            {
                Scope.Context.Register(Worker = _worker = new DomainWorker(workerName));
            }

            Scope.Context.Register(UpdateLoop = _updateLoop = new DefaultDomainUpdateLoopSubscriber(Scope.World));
        }

        protected override void DomainUpdate()
        {
            using (_worker.StartMonitoring(TargetDelta))
            {
                _updateLoop.Invoke(_worker.Elapsed, _worker.Delta);
            }

            if (Worker.Performance < 1)
            {
                Console.WriteLine(
                    $"Performance Problem! {(int)(Worker.Performance * 100)}% (Delta: {Worker.Delta.TotalMilliseconds}ms Target: {Worker.OptimalDeltaTarget.TotalMilliseconds}ms)"
                );
            }
        }
    }
}
