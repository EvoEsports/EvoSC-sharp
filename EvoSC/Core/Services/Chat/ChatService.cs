using System.Threading.Tasks;
using EvoSC.Domain;
using EvoSC.Interfaces.Chat;
using GbxRemoteNet;
using Microsoft.Extensions.Logging;
using ILogger = NLog.ILogger;

namespace EvoSC.Core.Services.Chat;

public class ChatService : IChatService
{
    private readonly ILogger<ChatService> _logger;
    private readonly DatabaseContext _databaseContext;
    private readonly GbxRemoteClient _gbxRemoteClient;
    public ChatService(ILogger<ChatService> logger, DatabaseContext databaseContext, GbxRemoteClient gbxRemoteClient)
    {
        _logger = logger;
        _databaseContext = databaseContext;
        _gbxRemoteClient = gbxRemoteClient;
    }
    
    public Task ClientOnPlayerChat(int playeruid, string login, string text, bool isregisteredcmd)
    {
        throw new System.NotImplementedException();
    }
}
