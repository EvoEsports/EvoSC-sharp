using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Controllers;

[Controller]
public class SpectatorTargetInfoEventController(ISpectatorTargetInfoService spectatorTargetInfoService)
    : EvoScController<EventControllerContext>
{
    // [Subscribe(ModeScriptEvent.GiveUp)]
    // public Task OnPlayerGiveUpAsync(object sender, PlayerUpdateEventArgs playerUpdateEventArgs) =>
    //     spectatorTargetInfoService.ForwardDnfToClientsAsync(playerUpdateEventArgs);

    /**
     *
     */
    
    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnectAsync(object x, PlayerConnectGbxEventArgs args)
    {
        if (!args.IsSpectator)
        {
            return;
        }

        //TODO: get spectator target
        //TODO: show widget
        
        // await spectatorTargetInfoService.SendManiaLinkAsync(args.Login);
    }
    
    [Subscribe(ModeScriptEvent.WayPoint)]
    public async Task OnWayPointAsync(object sender, WayPointEventArgs wayPointEventArgs)
    {
        await spectatorTargetInfoService.AddCheckpointAsync(
            wayPointEventArgs.Login,
            wayPointEventArgs.CheckpointInLap,
            wayPointEventArgs.LapTime
        );
        
        //TODO: get players spectating
        //TODO: send widget to spectating players
    }

    [Subscribe(ModeScriptEvent.StartRoundStart)]
    public async Task OnNewRoundAsync(object sender, RoundEventArgs roundEventArgs)
    {
        await spectatorTargetInfoService.ClearCheckpointsAsync();
        //TODO: update widgets for all spectating players
    }

    [Subscribe(GbxRemoteEvent.PlayerInfoChanged)]
    public async Task OnPlayerInfoChangedAsync(object sender, PlayerInfoChangedGbxEventArgs eventArgs)
    {
        var spectatorInfo = spectatorTargetInfoService.ParseSpectatorStatus(eventArgs.PlayerInfo.SpectatorStatus);
        var spectatorLogin = eventArgs.PlayerInfo.Login;

        if (spectatorInfo.IsSpectator)
        {
            await spectatorTargetInfoService.UpdateSpectatorTargetAsync(spectatorLogin, spectatorInfo.TargetPlayerId);
        }
        else
        {
            await spectatorTargetInfoService.RemovePlayerFromSpectatorsListAsync(spectatorLogin);
        }
    }
}
