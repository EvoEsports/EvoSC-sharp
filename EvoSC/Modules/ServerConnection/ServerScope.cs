using DefaultEcs;
using EvoSC.Core.Configuration;
using GameHost.V3;

namespace EvoSC.Modules.ServerConnection
{
    public class ServerScope : Scope
    {
        public readonly ServerConnectionConfig ConnectionConfig;
        public readonly World World;

        public ServerScope(Scope parent, World world, ServerConnectionConfig connectionConfig)
            : base(new ChildScopeContext(parent.Context))
        {
            Context.Register(World = world);
            Context.Register(ConnectionConfig = connectionConfig);
        }
    }
}