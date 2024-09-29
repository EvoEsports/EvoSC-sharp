using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Models;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;
using EvoSC.Modules.EvoEsports.ToornamentModule.Settings;
using EvoSC.Modules.Official.MatchReadyModule.Events;
using EvoSC.Modules.Official.MatchReadyModule.Events.Args;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Controllers;

[Controller]
public class MatchController(IMatchService matchService, IServerClient server, IToornamentSettings settings, ILogger<MatchController> logger) : EvoScController<IEventControllerContext>
{
    [Subscribe(MatchReadyEvents.AllPlayersReady)]
    public async Task OnAllPlayersReadyAsync(object sender, AllPlayersReadyEventArgs args)
    {
        if (!settings.AutomaticMatchStart)
        {
            return;
        }

        try
        {
            await matchService.StartMatchAsync();
        }
        catch (Exception ex)
        {
            await server.Chat.ErrorMessageAsync(
                "An error occured while trying to start the match. Contact match admin immediately.");
            logger.LogError(ex, "Failed to start match on all players ready.");
        }
    }

    [Subscribe(ModeScriptEvent.Scores)]
    public async Task OnMatchStateTracked(object sender, ScoresEventArgs args)
    {
        if (args.Section is not (ModeScriptSection.EndMatch))
        {
            return;
        }

        try
        {
            await matchService.EndMatchAsync(args);
        }
        catch (Exception ex)
        {
            await server.Chat.ErrorMessageAsync(
                "Failed to send match results. Take screenshots and contact a match admin.");
            logger.LogError(ex, "Failed to send match results.");
        }
    }

    [Subscribe(ModeScriptEvent.WarmUpStart)]
    public async Task OnWarmupStart(object sender, EventArgs args)
    {
        try
        {
            await matchService.FinishServerSetupAsync();
            await matchService.SetMatchGameMapAsync();
        }
        catch (Exception)
        {
            await server.Chat.ErrorMessageAsync("Failed to finish match setup. Contact a match admin immediately.");
            throw;
        }
    }

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task ForcePlayersIntoSpectate(object sender, PlayerConnectGbxEventArgs args)
    {
        try
        {
            if (args.IsSpectator)
            {
                return;
            }

            await matchService.ForcePlayerIntoSpectate(args.Login);
        }
        catch (Exception)
        {
            logger.LogWarning("Failed to put player {0} into spectate mode", args.Login);
            throw;
        }
    }
}
