using EvoSC.Common.Interfaces;
using EvoSC.Common.Services.Attributes;
using EvoSC.Modules.Official.ServerManagementModule.Events;
using EvoSC.Modules.Official.ServerManagementModule.Events.Args;
using EvoSC.Modules.Official.ServerManagementModule.Interfaces;

namespace EvoSC.Modules.Official.ServerManagementModule.Services;

[Service]
public class ServerManagementService(IServerClient serverClient, IEventManager events) : IServerManagementService
{
    public async Task SetPasswordAsync(string password)
    {
        await serverClient.Remote.SetServerPasswordAsync(password);
        await serverClient.Remote.SetServerPasswordForSpectatorAsync(password);

        await events.RaiseAsync(ServerManagementEvents.PasswordChanged,
            new PasswordChangedEventArgs { NewPassword = password });
    }

    public async Task SetMaxPlayersAsync(int maxPlayers)
    {
        await serverClient.Remote.SetMaxPlayersAsync(maxPlayers);
        await events.RaiseAsync(ServerManagementEvents.MaxPlayersChanged,
            new PlayerSlotsChangedEventArgs() { NewSlots = maxPlayers });
    }

    public async Task SetMaxSpectatorsAsync(int maxSpectators)
    {
        await serverClient.Remote.SetMaxSpectatorsAsync(maxSpectators);
        await events.RaiseAsync(ServerManagementEvents.MaxSpectatorsChanged,
            new PlayerSlotsChangedEventArgs() { NewSlots = maxSpectators });
    }
}
