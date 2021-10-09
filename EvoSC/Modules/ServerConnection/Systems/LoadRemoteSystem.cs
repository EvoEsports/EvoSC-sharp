using System;
using System.Collections.Generic;
using EvoSC.Core.Configuration;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.Module;
using NLog;

namespace EvoSC.Modules.ServerConnection
{
    public class LoadRemoteSystem : AppSystem
    {
        private HostRunnerScope _hostScope;
        private ILogger _logger;

        public LoadRemoteSystem(Scope scope) : base(scope)
        {
            Dependencies.AddRef(() => ref _hostScope);
            Dependencies.AddRef(() => ref _logger);
        }

        protected override void OnInit()
        {
            // TODO: Don't register UnsafeGbxConnector.Module here (it should be a separate assembly)
            HostModule.RegisterModule(_hostScope, sc => new EvoSC.Modules.Remotes.UnsafeGbxConnector.Module(sc));
            HostModule.RegisterModule(_hostScope, sc => new EvoSC.Modules.Remotes.GBXRemote.Module(sc));

            var targetRemote = HostModule.GetModuleGroupName(typeof(EvoSC.Modules.Remotes.GBXRemote.Module));
            if (_hostScope.Context.TryGet(out ControllerConfig config))
            {
                targetRemote = config.RemoteModule;
            }

            using var entitySet = _hostScope.World.GetEntities()
                .With<HostModuleDescription>()
                .AsSet();
        
            _logger.Info("Loading remote module '{0}'", targetRemote);

            var foundModule = false;
            foreach (var entity in entitySet.GetEntities())
            {
                if (!entity.Get<HostModuleDescription>().Match(targetRemote))
                    continue;

                var loadRequest = _hostScope.World.CreateEntity();
                loadRequest.Set(new RequestLoadModule("Load Remote", entity));

                foundModule = true;
            }

            if (!foundModule)
            {
                _logger.Error("Couldn't find remote module '{0}', loading default.", targetRemote);
                HostModule.LoadModule(_hostScope, sc => new EvoSC.Modules.Remotes.GBXRemote.Module(sc));
            }
        }
    }
}
