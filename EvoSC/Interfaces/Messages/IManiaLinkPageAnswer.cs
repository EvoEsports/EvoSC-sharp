using GbxRemoteNet.Structs;

namespace EvoSC.Interfaces.Messages;

public interface IManiaLinkPageAnswer : IServerMessage
{
    public string Answer { get; }
    public SEntryVal[] Entries { get; }
}
