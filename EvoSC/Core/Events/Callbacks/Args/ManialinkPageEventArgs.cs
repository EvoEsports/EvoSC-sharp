using System;
using System.Collections.Generic;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Messages;
using EvoSC.Interfaces.Players;

namespace EvoSC.Core.Events.Callbacks.Args;

public class ManialinkPageEventArgs : EventArgs
{
    public ManialinkPageEventArgs(IManiaLinkPageAnswer answer)
    {
        Player = answer.Player;
        Answer = answer;
    }

    public IPlayer Player { get;}
    
    public IManiaLinkPageAnswer Answer { get; }
}
