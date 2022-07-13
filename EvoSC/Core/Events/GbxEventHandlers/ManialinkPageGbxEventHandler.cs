using System.Threading.Tasks;
using EvoSC.Interfaces;
using EvoSC.Interfaces.Players;
using EvoSC.Interfaces.UI;
using GbxRemoteNet;
using GbxRemoteNet.Structs;

namespace EvoSC.Core.Events.GbxEventHandlers;

public class ManialinkPageGbxEventHandler : IGbxEventHandler
{
    private readonly IUiService _uiService;

    public ManialinkPageGbxEventHandler(IUiService uiService)
    {
        _uiService = uiService;
    }

    public void HandleEvents(GbxRemoteClient client)
    {
        client.OnPlayerManialinkPageAnswer += _uiService.OnPlayerManialinkPageAnswer;
    }
}
