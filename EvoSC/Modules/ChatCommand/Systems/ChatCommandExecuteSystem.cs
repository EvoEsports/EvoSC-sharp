using System;
using Collections.Pooled;
using DefaultEcs;
using EvoSC.Core.Remote;
using EvoSC.Events;
using EvoSC.ServerConnection;
using EvoSC.Utility.Commands;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.Utility;
using NLog;

namespace EvoSC.ChatCommand
{
    public class ChatCommandExecuteSystem : AppSystem
    {
        private World _world;
        private IGbxRemote _remote;

        private IServerEventLoopSubscriber _eventLoop;

        private ILogger _logger;

        public ChatCommandExecuteSystem(Scope scope) : base(scope)
        {
            Dependencies.AddRef(() => ref _world);
            Dependencies.AddRef(() => ref _remote);
            Dependencies.AddRef(() => ref _eventLoop);
            Dependencies.AddRef(() => ref _logger);
        }

        private EntitySet _commandSet;

        protected override void OnInit()
        {
            Disposables.AddRange(new IDisposable[]
            {
                _eventLoop.Subscribe(
                    OnEventLoop,
                    b => b.Position(OrderPosition.AtBeginning)
                ),
                _commandSet = _world.GetEntities()
                    .With<IsChatCommand>()
                    .AsSet()
            });
        }

        private void OnEventLoop()
        {
            foreach (var ev in _world.GetAll<EventOnPlayerChat>())
            {
                if (string.IsNullOrEmpty(ev.Text))
                    continue;

                if (!ev.Player.Backend.Has<InGamePlayerInfo>())
                {
                    _remote.ChatSendServerMessageToLoginAsync("Invalid Player", ev.Player.Login);

                    _logger.Warn(
                        "Player (Ent: {0} Login: {1}) has no {2}",
                        ev.Player.Backend,
                        ev.Player.Login,
                        nameof(InGamePlayerInfo)
                    );
                }

                using var matches = new PooledList<Entity>();
                if (!CommandUtility.GetBestCommands(_commandSet.GetEntities(), ev.Text, matches))
                {
                    if (ev.Text.Length >= 1 && ev.Text[0] == '/')
                    {
                        _remote.ChatSendServerMessageToLoginAsync("The command does not exist", ev.Player.Login);
                    }

                    continue;
                }

                var commandExecuted = false;

                using var arguments = new PooledList<Entity>();
                foreach (var command in matches)
                {
                    if (!CommandUtility.CanExecuteCommand(command, ev.Text, arguments))
                        continue;

                    command.Get<ChatCommandInvoked>()(ev.Player, arguments.Span);
                    commandExecuted = true;
                }

                if (!commandExecuted)
                {
                    if (ev.Text.Length >= 1 && ev.Text[0] == '/')
                    {
                        _remote.ChatSendServerMessageToLoginAsync("Your command was invalid", ev.Player.Login);
                    }
                }
            }
        }
    }
}
