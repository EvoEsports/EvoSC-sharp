using DefaultEcs;
using EvoSC.Core.Remote;
using GameHost.V3;

namespace EvoSC.ServerConnection
{
    public class ServerScope : Scope
    {
        public readonly IGbxRemote Remote;
        public readonly World World;

        public ServerScope(Scope parent, World world, IGbxRemote remote) : base(new ChildScopeContext(parent.Context))
        {
            Context.Register(World = world);
            Context.Register(Remote = remote);
        }
    }
}
