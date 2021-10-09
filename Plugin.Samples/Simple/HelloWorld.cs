using System;
using System.Threading;
using System.Threading.Tasks;
using GameHost.V3;
using EvoSC.Core.Plugins;
using EvoSC.Modules.ServerConnection;
using EvoSC.Utility.Remotes;

namespace Plugin.Samples.Simple
{
    //
    // PlayersConnection:
    //
    // Say Hello World! when created
    //
    public class HelloWorld : PluginSystemBase
    {
        public HelloWorld(Scope scope) : base(scope)
        {
        }

        protected override void OnInit()
        {
            Remote.SendChatMessage("Hello World!");
        }
    }
}