using EvoSC.Core.Commands.Chat.Interfaces;
using EvoSC.Core.Commands.Generic.Interfaces;

namespace EvoSC.Core.Commands.Chat;

public class ChatCommandGroup : ICommandGroup
{
    protected IChatCommandContext Context { get; private set; }

    void ICommandGroup.SetContext(ICommandContext context)
    {
        Context = (IChatCommandContext)context;
    }
}
