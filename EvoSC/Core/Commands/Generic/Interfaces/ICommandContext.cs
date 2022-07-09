using EvoSC.Interfaces.Players;
using GbxRemoteNet;

namespace EvoSC.Core.Commands.Generic.Interfaces;

public interface ICommandContext
{
    public GbxRemoteClient Client { get; }
    public IPlayer Player { get; }
}
