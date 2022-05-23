using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvoSC.Core.Events.Callbacks.Args;
using EvoSC.Core.Services.Players;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.UI;
using GbxRemoteNet;
using GbxRemoteNet.XmlRpc.ExtraTypes;
using GbxRemoteNet.XmlRpc.Packets;
using NLog;

namespace EvoSC.Core.Services.UI;

public class UiService : IUiService
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly GbxRemoteClient _gbxRemoteClient;
    private readonly IManialinkPageCallbacks _manialinkPageCallbacks;
    private static Dictionary<string, ManialinkAction> s_actions;

    public UiService(GbxRemoteClient gbxRemoteClient, IManialinkPageCallbacks iManialinkPageCallbacks)
    {
        _gbxRemoteClient = gbxRemoteClient;
        _manialinkPageCallbacks = iManialinkPageCallbacks;
        s_actions = new Dictionary<string, ManialinkAction>();
    }

    public async Task OnAnyCallback(MethodCall call, object[] param)
    {
        if (call.Method != "ManiaPlanet.PlayerManialinkPageAnswer")
        {
            return;
        }

        var player = await PlayerService.GetPlayer((string)param[1]);
        var values = new Dictionary<string, object>();
        foreach (Dictionary<string, object> para in (dynamic[])param[3])
        {
            var list = para.Values.ToList();
            if (!values.ContainsKey((string)list[0]))
            {
                values.Add((string)list[0], (string)list[1]);
            }
        }

        _manialinkPageCallbacks.OnPlayerManialinkPageAnswer(
            new ManialinkPageEventArgs(player, (string)param[2], values));

        var compare = s_actions.ContainsKey((string)param[2]);
        if (compare)
        {
            await s_actions[(string)param[2]].TriggerManialinkAction(values);
        }
    }

    public static string RegisterAction(ManialinkAction action)
    {
        s_actions.Add(action.Payload.UId, action);
        return action.Payload.UId;
    }

    public static void UnregisterAction(string ActionId)
    {
        s_actions.Remove(ActionId);
    }
}
