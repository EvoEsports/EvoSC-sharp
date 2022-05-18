using System;
using EvoSC.Core.Events.Callbacks.Args;
using GbxRemoteNet.Structs;

namespace EvoSC.Interfaces.UI;

public interface IManialinkPageCallbacks
{
    public event EventHandler<ManialinkPageEventArgs> PlayerManialinkPageAnswer;
    public void OnPlayerManialinkPageAnswer(ManialinkPageEventArgs e);
}
