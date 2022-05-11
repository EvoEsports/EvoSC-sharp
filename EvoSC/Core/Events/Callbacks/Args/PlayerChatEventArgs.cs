using System;
using EvoSC.Domain.Players;

namespace EvoSC.Core.Events.Callbacks.Args;

public class PlayerChatEventArgs : EventArgs
{
    public PlayerChatEventArgs(Player player, string text, bool isRegisteredCmd)
    {
        Player = player;
        Text = text;
        IsRegisteredCmd = isRegisteredCmd;
    }

    public Player Player { get; set; }
    public string Text { get; set; }
    public bool IsRegisteredCmd { get; set; }
}
