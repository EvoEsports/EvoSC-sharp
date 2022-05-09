using GbxRemoteNet;

namespace EvoSC.Interfaces;

public interface IGbxEventHandler
{
    public void HandleEvents(GbxRemoteClient client);
}
