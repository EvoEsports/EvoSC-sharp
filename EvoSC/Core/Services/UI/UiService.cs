using System;
using System.Threading.Tasks;
using EvoSC.Interfaces.UI;
using GbxRemoteNet;
using NLog;
using EvoSC.Domain.Players;

namespace EvoSC.Core.Services.UI;

public class UiService: IUiService
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly GbxRemoteClient _gbxRemoteClient;
    
    public UiService(GbxRemoteClient gbxRemoteClient)
    {
        _gbxRemoteClient = gbxRemoteClient;
    }

}
