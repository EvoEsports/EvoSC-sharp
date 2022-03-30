using System;

namespace EvoSC.Core.Events.Args;

public class PlayerDisconnectEventArgs : EventArgs
{
    public string Login { get; }
    public string Reason { get; }

    public PlayerDisconnectEventArgs(string login, string reason)
    {
        Login = login;
        Reason = reason;
    }
}
