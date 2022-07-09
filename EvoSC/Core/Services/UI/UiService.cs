using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvoSC.Core.Events.Callbacks.Args;
using EvoSC.Core.Services.Players;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Messages;
using EvoSC.Interfaces.Players;
using EvoSC.Interfaces.UI;
using GbxRemoteNet;
using GbxRemoteNet.Structs;
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

    public async Task OnPlayerManialinkPageAnswer(int playerUid, string login, string answer, SEntryVal[] entries)
    {
        var player = (IServerPlayer)await PlayerService.GetPlayer(login);
        var message = new ManialinkPageAnswer(player, answer, entries, playerUid);
        
        _manialinkPageCallbacks.OnPlayerManialinkPageAnswer(new ManialinkPageEventArgs(message));
        
        var values = new Dictionary<string, object>();
        foreach (var entry in entries)
        {
            if (!values.ContainsKey(entry.Name))
            {
                values.Add(entry.Name, entry.Value);
            }
        }
        
        var compare = s_actions.ContainsKey(answer);
        if (compare)
        {
            await s_actions[answer].TriggerManialinkAction(values);
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
