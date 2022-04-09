using System.Threading.Tasks;
using EvoSC.Interfaces.Chat;

namespace EvoSC.Core.Services.Chat;

public class ChatService : IChatService
{

    public ChatService()
    {
        
    }
    
    public Task ClientOnPlayerChat(int playeruid, string login, string text, bool isregisteredcmd)
    {
        throw new System.NotImplementedException();
    }
}
