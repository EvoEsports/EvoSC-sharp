using System.Threading.Tasks;
using DefaultEcs;
using GameHost.V3;
using EvoSC.Core.Plugins;
using EvoSC.Events;
using EvoSC.Modules.ServerConnection;
using EvoSC.Utility.Remotes;

namespace Plugin.Samples.ReactToEvents
{
    //
    // PlayersConnection:
    //
    // Subscribe to two events:
    // - EventOnPlayerConnect
    // - EventOnPlayerDisconnect
    //
    //  EventOnPlayerConnect:
    //      If the player is new to the server (since the controller lifetime) say a special message to them
    //      If the player isn't new, send a welcome back message.
    //
    //  EventOnPlayerDisconnect:
    //      Is a task based callback
    //      Send a bye message to the player
    //
    // ===================================================
    //  How to test?
    // ===================================================
    //
    //  As player join or leave the server.
    //
    //  Joining for the first time:
    //      Welcome 'Your Name' to the server!
    //  Joining for the second time:
    //      Welcome back 'Your Name'!
    //  Leaving:
    //      Bye bye 'Your Name'!
    //

    public class PlayersConnection : PluginSystemBase
    {
        public PlayersConnection(Scope scope) : base(scope)
        {
        }

        protected override void OnInit()
        {
        }

        [ServerEvent]
        private void OnPlayerConnected(Entity entity, EventOnPlayerConnect ev)
        {
            if (ev.Player.Entity.Has<WasConnectedInThePast>())
            {
                Remote.SendChatMessage($"Welcome back $<{ev.Player.NickName}$z$s$>!");
            }
            else
            {
                Remote.SendChatMessage($"Welcome to the server $<{ev.Player.NickName}$z$s$>!");

                ev.Player.Entity.Set<WasConnectedInThePast>();
            }
        }

        [ServerEvent]
        private void OnPlayerDisconnected(Entity entity, EventOnPlayerDisconnect ev)
        {
            Remote.SendChatMessage($"Bye bye $<{ev.Player.NickName}$z$s$>!");
        }

        private struct WasConnectedInThePast
        {

        }
    }
}