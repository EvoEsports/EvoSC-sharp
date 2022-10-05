using GbxRemoteNet;

namespace EvoSC.Common.Interfaces;

public interface IServerClient
{
    public GbxRemoteClient Remote { get; }

    public Task StartAsync(CancellationToken token);
    public Task StopAsync(CancellationToken token);
}
