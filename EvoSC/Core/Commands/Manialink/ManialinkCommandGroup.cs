using EvoSC.Core.Commands.Chat.Interfaces;
using EvoSC.Core.Commands.Generic.Interfaces;
using EvoSC.Core.Commands.Manialink.Interfaces;

namespace EvoSC.Core.Commands.Manialink;

public class ManialinkCommandGroup : ICommandGroup
{
    protected IManialinkCommandContext Context { get; private set; }

    public void SetContext(ICommandContext context)
    {
        Context = (IManialinkCommandContext)context;
    }
}
