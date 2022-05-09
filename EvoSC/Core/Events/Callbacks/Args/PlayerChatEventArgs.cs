using System;

namespace EvoSC.Core.Events.Callbacks.Args;

public class PlayerChatEventArgs : EventArgs
{
    public int PlayerUid { get; set; }
    public string Login { get; set; }
    public string Text { get; set; }
    public bool IsRegisteredCmd { get; set; }

    public PlayerChatEventArgs(int playerUid, string login, string text, bool isRegisteredCmd)
    {
        PlayerUid = playerUid;
        Login = login;
        Text = text;
        IsRegisteredCmd = isRegisteredCmd;
    }
}
