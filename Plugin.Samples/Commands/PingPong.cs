using System;
using DefaultEcs;
using GameHost.V3;
using EvoSC.ChatCommand;
using EvoSC.Core.Plugins;
using EvoSC.Core.Remote;
using EvoSC.ServerConnection;

namespace Plugin.Samples.Commands
{
    //
    // The PingPong system use the raw methods to register chat commands
    //
    // Check other systems such as:
    //  - SendGlobalMessage
    //  - SendPrivateMessage
    //
    // To see how we register commands much more simply.
    //
    // ===================================================
    //  How to test?
    // ===================================================
    //
    //  As player, type 'ping' in the chat.
    //  It should return a result:
    //      'Your nickname' 'Your server Id' 'Your login' pong!
    //

    public class PingPong : PluginSystemBase
    {
        public PingPong(Scope scope) : base(scope)
        {
        }

        [Dependency] private ChatCommandManager _chatCommand { get; }
        [Dependency] private IGbxRemote _remote { get; }

        protected override void OnInit()
        {
            Disposables.Add(_chatCommand.Add("ping", OnPingCommand, "Ping Pong!", hidden: true));
        }

        private void OnPingCommand(PlayerEntity player, Span<Entity> arguments)
        {
            _remote.ChatSendServerMessageAsync($"$<{player.Info.Result.NickName}$> {player.Id} {player.Login} pong!");
        }
    }
}