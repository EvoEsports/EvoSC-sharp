using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Events.CoreEvents;

public class ChatMessageEventArgs : EventArgs
{
    public required IPlayer Player { get; set; }
    public required string MessageText { get; set; }
}
