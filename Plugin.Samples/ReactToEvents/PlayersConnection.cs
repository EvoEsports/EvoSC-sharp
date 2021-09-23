using System.Threading.Tasks;
using DefaultEcs;
using GameHost.V3;
using EvoSC.Core.Plugins;
using EvoSC.Core.Remote;
using EvoSC.Events;
using EvoSC.ServerConnection;

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
    //      Is a task based callback
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
        [Dependency] private IGbxRemote _remote;

        public PlayersConnection(Scope scope) : base(scope)
        {
        }

        protected override void OnInit()
        {
        }

        [ServerEvent]
        private async Task OnPlayerConnected(Entity entity, EventOnPlayerConnect component)
        {
            var playerInfo = await component.Player.Info;
            if (component.Player.Backend.Has<WasConnectedInThePast>())
            {
                await _remote.ChatSendServerMessageAsync($"Welcome back $<{playerInfo.NickName}$z$s$>!");
            }
            else
            {
                await _remote.ChatSendServerMessageAsync($"Welcome to the server $<{playerInfo.NickName}$z$s$>!");

                component.Player.Backend.Set<WasConnectedInThePast>();
            }
        }

        [ServerEvent]
        private async Task OnPlayerDisconnected(Entity entity, EventOnPlayerDisconnect component)
        {
            var playerInfo = await component.Player.Info;
            await _remote.ChatSendServerMessageAsync($"Bye bye $<{playerInfo.NickName}$z$s$>!");
        }

        private struct WasConnectedInThePast
        {

        }
    }
}