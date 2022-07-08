using System;
using EvoSC.Domain.Players;

namespace EvoSC.Core.Events.Callbacks.Args;

public class PlayerChatEventArgs : EventArgs
{
    public PlayerChatEventArgs(DatabasePlayer databasePlayer, string text, bool isRegisteredCmd)
    {
        DatabasePlayer = databasePlayer;
        Text = text;
        IsRegisteredCmd = isRegisteredCmd;
    }

    public DatabasePlayer DatabasePlayer { get; set; }

    public string Text { get; set; }

    public bool IsRegisteredCmd { get; set; }
}
