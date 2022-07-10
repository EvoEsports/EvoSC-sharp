using GbxRemoteNet.Structs;

namespace EvoSC.Interfaces.Messages;

public interface IManiaLinkPageAnswer : IServerMessage
{
    public SEntryVal[] Entries { get; }
}
