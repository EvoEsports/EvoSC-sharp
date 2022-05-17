using System;
using System.Collections.Generic;
using EvoSC.Domain.Players;

namespace EvoSC.Core.Events.Callbacks.Args;

public class ManialinkPageEventArgs : EventArgs
{
    public ManialinkPageEventArgs(Player player, string answer, Dictionary<string, object> entries)
    {
        Player = player;
        Answer = answer;
        Entries = entries;
    }

    public Player Player { get; set; }
    public string Answer { get; set; }
    public Dictionary<string, object> Entries { get; set; }
}
