using System;
using DefaultEcs;
using GameHost.V3;
using GameHost.V3.Domains.Time;
using EvoSC.Core.Plugins;
using EvoSC.Events;
using EvoSC.Modules.Remotes.GBXRemote.Impl;
using EvoSC.Modules.ServerConnection;
using EvoSC.Utility.Remotes;
using GbxRemoteNet;

namespace Plugin.Samples.ReactToEvents
{
    //
    // ChatMessage:
    //
    // Subscribe to one event:
    // - EventOnPlayerChat
    //
    //  EventOnPlayerChat:
    //      Log to the console the current time since the app started, the login of the player and the written text in the chat.
    //
    // ===================================================
    //  How to test?
    // ===================================================
    //
    //  As player send whatever message in the chat.
    //
    //  The console should write:
    //      <42s> 'Your Login': 'Your Message'
    //

    public class ChatMessage : PluginSystemBase
    {
        [Dependency] private IManagedWorldTime _worldTime;

        public ChatMessage(Scope scope) : base(scope)
        {
        }

        protected override void OnInit()
        {
        }

        [ServerEvent]
        private void OnPlayerChat(EventOnPlayerChat ev)
        {
            if (ev.Player.Entity.Has<IsServerPlayer>())
                return;
            Logger.Info("<{0}s> {1}: {2}", _worldTime.Total.TotalSeconds, ev.Player.Login, ev.Text);
        }
    }
}