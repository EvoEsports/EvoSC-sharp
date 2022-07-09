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
        // @todo change to appropriate callback when it becomes available from gbxclient.net
        // client.OnAnyCallback += _uiService.OnAnyCallback;
        client.OnPlayerManialinkPageAnswer += _uiService.OnPlayerManialinkPageAnswer;
    }
}
