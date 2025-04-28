using EvoSC.Modules.Attributes;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule;

[Module(IsInternal = true)]
public class ServerSyncModule(INatsConnectionService natsConnection) : EvoScModule, IToggleable
{
    public async Task EnableAsync()
    {
        await natsConnection.ConnectAsync();
    }

    public async Task DisableAsync()
    {
        await natsConnection.DisposeAsync();
    }
}
