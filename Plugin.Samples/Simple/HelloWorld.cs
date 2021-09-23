using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection;
using EvoSC.Core.Plugins;
using EvoSC.Core.Remote;

namespace Plugin.Samples.Simple
{
    //
    // PlayersConnection:
    //
    // Say Hello World! when created
    //
    public class HelloWorld : PluginSystemBase
    {
        [Dependency] private IGbxRemote _remote;
        
        public HelloWorld(Scope scope) : base(scope)
        {
        }

        protected override void OnInit()
        {
            _remote.ChatSendServerMessageAsync("Hello World!");
        }
    }
}