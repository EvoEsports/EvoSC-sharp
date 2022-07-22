using EvoSC.Core.Commands.Generic.Interfaces;
using EvoSC.Interfaces.Messages;
using EvoSC.Interfaces.Players;

namespace EvoSC.Core.Commands.Manialink.Interfaces;

public interface IManialinkCommandContext : ICommandContext
{
    public IManiaLinkPageAnswer Answer { get; }
}
