using System;
using System.Reflection;
using System.Threading.Tasks;
using DefaultEcs;
using EvoSC.Core.Plugins;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection;
using GameHost.V3.Threading;
using GameHost.V3.Utility;
using JetBrains.Annotations;

namespace EvoSC.ServerConnection
{
    [AttributeUsage(AttributeTargets.Method)]
    [MeansImplicitUse]
    public class ServerEventAttribute : CustomInjectorAttribute
    {
        private static void ThrowOnInvalidParameter(MethodInfo methodInfo, ParameterInfo parameter, bool oneParam)
        {
            if (parameter.ParameterType == typeof(Entity))
            {
                if (oneParam)
                {
                    throw new InvalidOperationException(
                        $"{methodInfo} has only one parameter. But it shouldn't be an entity"
                    );
                }

                throw new InvalidOperationException(
                    $"{methodInfo} second parameter shouldn't be an entity"
                );
            }

            if (!parameter.ParameterType.IsValueType)
            {
                throw new InvalidOperationException(
                    $"{methodInfo} parameter should be a value type (struct)"
                );
            }
        }

        public override void Invoke(IReadOnlyContext context, object system, object member)
        {
            if (system is not AppSystem appSystem)
                throw new InvalidOperationException($"{system.GetType()} != {typeof(AppSystem)}");

            var disposables = appSystem.Disposables;

            if (!context.TryGet(out World world))
                throw new InvalidOperationException($"{nameof(World)} not found");

            if (!context.TryGet(out IServerEventLoopSubscriber eventLoop))
                throw new InvalidOperationException($"{nameof(IServerEventLoopSubscriber)} not found");

            var method = (MethodInfo)member;
            var parameters = method.GetParameters();

            EntitySet eventSet;
            Func<Entity, object> getComponent;
            Action finalAction;

            void Set(ParameterInfo parameter)
            {
                eventSet = world.GetEntities()
                    .With(parameter.ParameterType)
                    .AsSet();

                getComponent = CreateConvertAction(parameter.ParameterType);
            }

            var isAsync = method.ReturnType == typeof(Task);
            if (isAsync)
            {
                if (!context.TryGet(out ConstrainedTaskScheduler taskScheduler))
                    throw new InvalidOperationException($"{nameof(ConstrainedTaskScheduler)} not found");

                switch (parameters.Length)
                {
                    case 1:
                        ThrowOnInvalidParameter(method, parameters[0], true);
                        Set(parameters[0]);
                        finalAction = () =>
                        {
                            foreach (var entity in eventSet.GetEntities())
                            {
                                var component = getComponent(entity);
                                taskScheduler.StartUnwrap(async () =>
                                {
                                    await ((Task)method.Invoke(system, new[] {component}))!;
                                });
                            }
                        };

                        break;
                    case 2:
                        ThrowOnInvalidParameter(method, parameters[1], true);
                        Set(parameters[1]);
                        finalAction = () =>
                        {
                            foreach (var entity in eventSet.GetEntities())
                            {
                                var e = entity;
                                var component = getComponent(entity);
                                taskScheduler.StartUnwrap(async () =>
                                {
                                    await ((Task)method.Invoke(system, new[] {e, component}))!;
                                });
                            }
                        };

                        break;
                    default:
                        throw new InvalidOperationException(
                            $"{nameof(ServerEventAttribute)} only accept 1 or 2 parameters"
                        );
                }
            }
            else
            {
                switch (parameters.Length)
                {
                    case 1:
                        ThrowOnInvalidParameter(method, parameters[0], true);
                        Set(parameters[0]);
                        finalAction = () =>
                        {
                            foreach (var entity in eventSet.GetEntities())
                            {
                                var component = getComponent(entity);
                                method.Invoke(system, new[] {component});
                            }
                        };

                        break;
                    case 2:
                        ThrowOnInvalidParameter(method, parameters[1], true);
                        Set(parameters[1]);
                        finalAction = () =>
                        {
                            foreach (var entity in eventSet.GetEntities())
                            {
                                var component = getComponent(entity);
                                method.Invoke(system, new[] {entity, component});
                            }
                        };

                        break;
                    default:
                        throw new InvalidOperationException(
                            $"{nameof(ServerEventAttribute)} only accept 1 or 2 parameters"
                        );
                }
            }

            disposables.Add(eventLoop.Subscribe(finalAction));
            disposables.Add(eventSet);
        }

        static Func<Entity, object> GetConvertAction<T>() => e => e.Get<T>();

        // super fast way of converting entity data to a simple object
        static Func<Entity, object> CreateConvertAction(Type type)
        {
            return (Func<Entity, object>)
                typeof(ServerEventAttribute)
                    .GetMethod(nameof(GetConvertAction), BindingFlags.Static | BindingFlags.NonPublic)!
                    .MakeGenericMethod(type)
                    .Invoke(null, null);
        }
    }
}
