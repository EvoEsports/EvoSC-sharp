using GameHost.V3;
using EvoSC.ChatCommand;
using EvoSC.Core.Plugins;
using EvoSC.Core.Remote;
using EvoSC.ServerConnection;

namespace Plugin.Samples.SimpleCommand
{
    //
    // SendPrivateMessage:
    //
    // Create one command via the custom injector 'ChatCommandAttribute'
    // - OnGlobalCommand /global string:Message
    //
    //  OnGlobalCommand:
    //      Send a global message on the server. The sender will be unknown.
    //
    // ===================================================
    //  How to test?
    // ===================================================
    //
    //  As player, start writing '/global' in the chat, then add your message enclosed in "".
    //  example:
    //      /global "Hello World!"
    //
    //  It should return a result:
    //      Global: Your Message
    //

    public class SendGlobalMessage : PluginSystemBase
    {
        [Dependency] private IGbxRemote _remote;

        public SendGlobalMessage(Scope scope) : base(scope)
        {
        }

        protected override void OnInit()
        {
        }

        [ChatCommand("/global")]
        private void OnGlobalCommand(PlayerEntity playerEntity, string message)
        {
            _remote.ChatSendServerMessageAsync($"$ff0Global: $ffd$i{message}");
        }
    }
}