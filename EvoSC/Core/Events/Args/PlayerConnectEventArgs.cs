using System;

namespace EvoSC.Core.Events.Args;

public class PlayerConnectEventArgs: EventArgs
{
    public string Login { get; set; }
    public bool IsSpectator { get; set; }

    public PlayerConnectEventArgs(string login, bool isSpectator)
    {
        Login = login;
        IsSpectator = isSpectator;
    }
}
