using System;
using EvoSC.Core.Events.Callbacks.Args;
using EvoSC.Interfaces.UI;
using GbxRemoteNet.Structs;

namespace EvoSC.Core.Events.Callbacks;

public class ManialinkPageCallbacks : IManialinkPageCallbacks
{
    public event EventHandler<ManialinkPageEventArgs> PlayerManialinkPageAnswer;
    
    public virtual void OnPlayerManialinkPageAnswer(ManialinkPageEventArgs e)
    {
        PlayerManialinkPageAnswer?.Invoke(this, e);
    }
    
}
