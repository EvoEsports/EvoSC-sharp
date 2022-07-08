using System;
using System.Threading.Tasks;

namespace EvoSC.Interfaces.Chat;

public interface IChatService
{
    public Task ClientOnPlayerChat(int playeruid, string login, string text, bool isregisteredcmd);
    /// <summary>
    /// Triggered when a server message from the server is sent.
    /// </summary>
    public event Func<IServerServerChatMessage, Task> ServerChatMessage;
}
