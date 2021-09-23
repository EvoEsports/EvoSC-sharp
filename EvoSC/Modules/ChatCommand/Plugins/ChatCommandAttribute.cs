using System;
using System.Collections.Generic;
using System.Reflection;
using DefaultEcs;
using EvoSC.Core.Plugins;
using EvoSC.ServerConnection;
using EvoSC.Utility.Commands;
using GameHost.V3.Ecs;
using GameHost.V3.Injection;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.Utility;
using JetBrains.Annotations;
using EvoSC.ChatCommand;

namespace EvoSC.ChatCommand
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
    [MeansImplicitUse]
    public class ChatCommandAttribute : CustomInjectorAttribute
    {
        public string Description;
        public string Name;
        public bool Hidden;

        public ChatCommandAttribute(string name = "", string description = "", bool hidden = false)
        {
            Description = description;
            Name = name;
            Hidden = hidden;
        }

        public override void Invoke(IReadOnlyContext context, object system, object member)
        {
            if (system is not AppSystem appSystem)
            {
                throw new InvalidOperationException(
                    $"{nameof(ChatCommandAttribute)} does not work on {system.GetType()}"
                );
            }

            if (!context.TryGet(out IDependencyResolver resolver))
            {
                throw new InvalidOperationException(
                    $"Immediate dependency {resolver} not found on system {system}"
                );
            }

            var dependencyCollection = new DependencyCollection(context, resolver, $"{system}/ChatCommandDep");
            dependencyCollection.Add(new Dependency(typeof(ChatCommandManager)));
            dependencyCollection.OnComplete(deps =>
            {
                var chatCommandManager = (ChatCommandManager)deps[0];

                var field = member as FieldInfo;
                var property = member as PropertyInfo;
                var method = member as MethodInfo;

                if (field != null) InvokeOnField(field, chatCommandManager, appSystem);
                if (property != null) InvokeOnProperty(property, chatCommandManager, appSystem);
                if (member != null) InvokeOnMethod(method, chatCommandManager, appSystem);
            });
        }
        
        static Func<Entity, object> GetConvertAction<T>() => e => e.Get<T>();

        // super fast way of converting entity data to a simple object
        static Func<Entity, object> CreateConvertAction(Type type)
        {
            return (Func<Entity, object>)
                typeof(ChatCommandAttribute)
                    .GetMethod(nameof(GetConvertAction), BindingFlags.Static | BindingFlags.NonPublic)!
                    .MakeGenericMethod(type)
                    .Invoke(null, null);
        }

        private void InvokeOnField(FieldInfo fieldInfo, ChatCommandManager chatCommand, AppSystem appSystem)
        {
            if (string.IsNullOrEmpty(Name))
                Name = $"/{fieldInfo.Name}";

            var getComponent = CreateConvertAction(fieldInfo.FieldType);

            appSystem.Disposables.AddRange(new IDisposable[]
            {
                chatCommand.Add(Name, (_, args) =>
                    {
                        fieldInfo.SetValue(appSystem, getComponent.Invoke(args[0]));
                    },
                    Description, new[] {new CommandArguments.Argument(fieldInfo.FieldType, fieldInfo.Name)},
                    hidden: Hidden)
            });
        }

        private void InvokeOnProperty(PropertyInfo propertyInfo, ChatCommandManager chatCommand, AppSystem appSystem)
        {
            if (string.IsNullOrEmpty(Name))
                Name = $"/{propertyInfo.Name}";

            var getComponent = CreateConvertAction(propertyInfo.PropertyType);

            appSystem.Disposables.AddRange(new IDisposable[]
            {
                chatCommand.Add(Name, (_, args) =>
                    {
                        propertyInfo.SetValue(appSystem, getComponent.Invoke(args[0]));
                    },
                    Description,
                    new[] {new CommandArguments.Argument(propertyInfo.PropertyType, propertyInfo.Name)},
                    hidden: Hidden)
            });
        }

        private void InvokeOnMethod(MethodInfo methodInfo, ChatCommandManager chatCommand, AppSystem appSystem)
        {
            if (string.IsNullOrEmpty(Name))
                Name = $"/{methodInfo.Name}";

            var arguments = new List<CommandArguments.Argument>();
            var getComponents = new List<Func<Entity, object>>();

            var hasPlayerEntityAsArg = false;
            foreach (var parameter in methodInfo.GetParameters())
            {
                // TODO: add some attributes to parameters (for setting custom name or description)
                if (parameter.ParameterType != typeof(PlayerEntity))
                {
                    arguments.Add(new CommandArguments.Argument(parameter.ParameterType, parameter.Name));
                    getComponents.Add(CreateConvertAction(parameter.ParameterType));
                }
                else
                    hasPlayerEntityAsArg = true;
            }

            ChatCommandInvoked callback;
            if (hasPlayerEntityAsArg)
            {
                callback = (entity, args) =>
                {
                    var parameters = new object[arguments.Count + 1];
                    parameters[0] = entity;

                    for (var i = 0; i < args.Length; i++)
                    {
                        parameters[i + 1] = getComponents[i].Invoke(args[i]);
                    }

                    methodInfo.Invoke(appSystem, parameters);
                };
            }
            else
            {
                callback = (_, args) =>
                {
                    var parameters = new object[arguments.Count];
                    for (var i = 0; i < args.Length; i++)
                    {
                        parameters[i] = getComponents[i].Invoke(args[i]);
                    }

                    methodInfo.Invoke(appSystem, parameters);
                };
            }

            appSystem.Disposables.AddRange(new IDisposable[]
            {
                chatCommand.Add(Name, callback, Description, arguments.ToArray(), hidden: Hidden)
            });
        }
    }
}
