using System.Threading.Tasks;

namespace EvoSC.Interfaces.Chat;

public interface IChatService
{
    public Task ClientOnPlayerChat(int playeruid, string login, string text, bool isregisteredcmd);
}
