using System;
using System.Collections.Generic;
using System.Linq;
using DefaultEcs;
using EvoSC.Core.Configuration;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.Module;
using GameHost.V3.Threading;
using NLog;

namespace EvoSC.Core.Systems
{
    public class LoadModulesListSystem : AppSystem
    {
        private readonly HashSet<Entity> _loaded = new();
        
        private World _world;
        private ILogger _logger;

        private IScheduler _scheduler;

        public LoadModulesListSystem(Scope scope) : base(scope)
        {
            Dependencies.AddRef(() => ref _world);
            Dependencies.AddRef(() => ref _logger);
            Dependencies.AddRef(() => ref _scheduler);
        }

        private ModuleListConfig _config;

        protected override void OnInit()
        {
            if (!_world.Has<ModuleListConfig>())
            {
                _logger.Info($"No modules to autoload. ('Config/modules.json' not found)");
                return;
            }

            _config = _world.Get<ModuleListConfig>();
            if (_config.Modules.Length == 0)
            {
                _logger.Info($"No modules to autoload. (Field 'load' empty)");
                return;
            }

            Disposables.Add(
                _world.SubscribeComponentAdded(
                    new ComponentAddedHandler<HostModuleDescription>(OnRegisteredModuleAdded)
                )
            );

            using var set = _world.GetEntities()
                .With<HostModuleDescription>()
                .AsSet();

            foreach (var entity in set.GetEntities())
            {
                TryLoadModule(entity, entity.Get<HostModuleDescription>());
            }
        }

        private void TryLoadModule(Entity entity, HostModuleDescription description)
        {
            if (_loaded.Contains(entity))
                return;
            
            foreach (var module in _config.Modules)
            {
                if (!description.Match(module))
                    continue;

                if (_logger.IsDebugEnabled)
                    _logger.Debug("AutoLoad module '{0}'", description.ToPath());

                var request = _world.CreateEntity();
                request.Set(new RequestLoadModule($"AutoLoad {module}", entity));

                _loaded.Add(entity);

                break;
            }
        }

        private void OnRegisteredModuleAdded(in Entity entity, in HostModuleDescription description)
        {
            _scheduler.Add(args =>
            {
                args.t.TryLoadModule(args.entity, args.description);
                return true;
            }, (t: this, entity, description), default);
        }
    }
}
