using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using DefaultEcs;
using EvoSC.ServerConnection;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.Threading;
using GameHost.V3.Utility;
using NLog;

namespace EvoSC.Core.Plugins
{
    public delegate void OnServerEvent<in TComponent>(Entity entity, TComponent component);

    public delegate Task OnServerEventAsync<in TComponent>(Entity entity, TComponent component);

    public abstract class PluginSystemBase : AppSystem
    {
        private IServerEventLoopSubscriber _eventLoop;
        private ILogger _logger;

        private readonly Scope _scope;
        private ConstrainedTaskScheduler _taskScheduler;
        private World _world;

        protected PluginSystemBase(Scope scope) : base(scope)
        {
            _scope = scope;
            
            Dependencies.AddRef(() => ref _logger);
            Dependencies.AddRef(() => ref _eventLoop);
            Dependencies.AddRef(() => ref _world);
            Dependencies.AddRef(() => ref _taskScheduler);

            foreach (var member in GetType()
                .GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (member.GetCustomAttribute<DependencyAttribute>() != null)
                {
                    Dependencies.Add(new ReflectionDependency(member, this));
                }
            }
        }

        protected ILogger Logger => _logger;

        protected override void OnDependenciesResolved(IReadOnlyList<object> dependencies)
        {
            base.OnDependenciesResolved(dependencies);
            
            // Process dependencies first, and then custom injectors
            foreach (var member in GetType()
                .GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                var customInjector = member.GetCustomAttribute<CustomInjectorAttribute>(true);
                if (customInjector != null)
                {
                    Logger.Trace("Injector {0} found for {1}", customInjector, member.Name);
                    customInjector.Invoke(_scope.Context, this, member);
                }
            }
        }
    }
}
