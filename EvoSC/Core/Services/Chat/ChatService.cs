using System.Threading.Tasks;
using EvoSC.Domain;
using EvoSC.Interfaces.Chat;
using GbxRemoteNet;
using NLog;

namespace EvoSC.Core.Services.Chat;

public class ChatService : IChatService
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly DatabaseContext _databaseContext;
    private readonly GbxRemoteClient _gbxRemoteClient;
    public ChatService(DatabaseContext databaseContext, GbxRemoteClient gbxRemoteClient)
    {

        _databaseContext = databaseContext;
        _gbxRemoteClient = gbxRemoteClient;
    }

    public Task ClientOnPlayerChat(int playeruid, string login, string text, bool isregisteredcmd)
    {
        return Task.CompletedTask;
    }
}
