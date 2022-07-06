using System.Threading.Tasks;

namespace EvoSC.Core.Services.Commands;

public class CommandBase
{
    public CommandBase(CommandContext context)
    {
        Context = context;
    }

    protected CommandContext Context { get; }

    protected Task ReplyAsync(string message)
    {
        return Context.RemoteClient.ChatSendServerMessageToLoginAsync(message, Context.Player.Login);
    }
}
