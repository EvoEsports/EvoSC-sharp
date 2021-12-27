using EvoSC.Core.Plugins;
using EvoSC.Modules.ChatCommand.Plugins;
using EvoSC.Modules.ServerConnection;
using EvoSC.Utility.Remotes;
using GameHost.V3;

namespace Plugin.Samples.Commands
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
        public SendGlobalMessage(Scope scope) : base(scope)
        {
        }

        protected override void OnInit()
        {
        }

        [ChatCommand("/global")]
        private void OnGlobalCommand(PlayerEntity playerEntity, string message)
        {
            Remote.SendChatMessage($"$ff0Global: $ffd$i{message}");
        }
    }
}