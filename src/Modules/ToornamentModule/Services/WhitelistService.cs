using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Util;
using EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;
using EvoSC.Modules.EvoEsports.ToornamentModule.Settings;
using GbxRemoteNet;
using GbxRemoteNet.Structs;
using Microsoft.Extensions.Logging;
using ToornamentApi.Models.Api.TournamentApi;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Services;

public class WhitelistService(
    ILogger<WhitelistService> logger,
    IServerClient server,
    IToornamentSettings settings,
    IStateService stateService,
    IMatchPlayerService matchPlayerService,
    IPlayerManagerService playerManagerService,
    IPermissionManager permissionManager
    ) : IWhitelistService
{
    

    public async Task WhitelistPlayers(OpponentInfo[] opponents)
    {
        logger.LogDebug("Begin of WhitelistPlayers()");
        var multiCall = new MultiCall();

        var players = await matchPlayerService.GetPlayersFromOpponents(opponents);

        foreach (var player in players)
        {
            var login = PlayerUtils.ConvertAccountIdToLogin(player.AccountId);
            multiCall.Add("AddGuest", login);
        }

        await server.Remote.MultiCallAsync(multiCall);
        logger.LogDebug("End of WhitelistPlayers()");
    }

    public async Task WhitelistSpectators()
    {
        logger.LogDebug("Begin of WhitelistSpectators()");
        if (!string.IsNullOrEmpty(settings.Whitelist))
        {
            var multiCall = new MultiCall();
            foreach (var accountId in settings.Whitelist.Split(','))
            {
                var login = PlayerUtils.ConvertAccountIdToLogin(accountId);
                multiCall.Add("AddGuest", login);
            }

            await server.Remote.MultiCallAsync(multiCall);
        }

        logger.LogDebug("End of WhitelistSpectators()");
    }
    
    

    public async Task ForcePlayerIntoSpectate(string login)
    {
        if (!settings.UseExperimentalFeatures)
        {
            return;
        }

        if (!stateService.WaitingForMatchStart && !stateService.MatchInProgress)
        {
            // No match in progress, so players won't get put into Spectate
            return;
        }

        var accountId = PlayerUtils.ConvertLoginToAccountId(login);
        var player = await playerManagerService.GetOrCreatePlayerAsync(accountId);

        var guestList = await GetGuestListAsync();
        var whitelistedSpectates = settings.Whitelist.Split(',');

        //Player is not in the guestlist -> will get kicked
        if (guestList.FirstOrDefault(g => g.Login == login) is null)
        {
            await KickAsync(player);
        }

        //Player is in the configured whitelist -> put into spectate mode
        if (player is not null && whitelistedSpectates.Contains(accountId))
        {
            if (player.Groups.Count() == 0)
            {
                await permissionManager.AddPlayerToGroupAsync(player, settings.DefaultGroupId);
            }

            await ForceSpectatorAsync(player);
        }
    }
    
    public async Task KickNonWhitelistedPlayers()
    {
        if (!settings.UseExperimentalFeatures)
        {
            return;
        }

        var connectedPlayers = await server.Remote.GetPlayerListAsync();

        var guestList = await GetGuestListAsync();
        var whitelistedSpectates = settings.Whitelist.Split(',');

        foreach (var connectedPlayer in connectedPlayers)
        {
            if (connectedPlayer.IsServer())
            {
                continue;
            }

            var accountId = PlayerUtils.ConvertLoginToAccountId(connectedPlayer.Login);
            var player = await playerManagerService.GetOrCreatePlayerAsync(accountId);

            //Skip player if the player is Admin
            if (player.Groups.Any(x => x.Id == 1))
            {
                continue;
            }

            //Player is not in the guestlist -> will get kicked
            if (guestList.FirstOrDefault(g => g.Login == connectedPlayer.Login) is null)
            {
                await KickAsync(player);
            }

            //Player is in the configured whitelist -> put into spectate mode
            if (player is not null && whitelistedSpectates.Contains(accountId))
            {
                if (player.Groups.Count() == 0)
                {
                    await permissionManager.AddPlayerToGroupAsync(player, settings.DefaultGroupId);
                }

                await ForceSpectatorAsync(player);
            }
        }
    }
    
    private async Task KickAsync(IPlayer player)
    {
        if (await server.Remote.KickAsync(player.GetLogin(), ""))
        {
            logger.LogDebug(
                "Kicked player {0} from server, because player was not whitelisted or expected as a participant of this match",
                player.UbisoftName);
        }
        else
        {
            logger.LogWarning("Failed to kick player {0} from server", player.UbisoftName);
        }
    }

    private Task<bool> ForceSpectatorAsync(IPlayer player) => server.Remote.ForceSpectatorAsync(player.GetLogin(), 1);


    private async Task<TmGuestListEntry[]> GetGuestListAsync()
    {
        var maxPlayers = await server.Remote.GetMaxPlayersAsync();
        var nrOfPlayers = 0;
        if (maxPlayers is not null)
        {
            nrOfPlayers += maxPlayers.CurrentValue;
        }

        var whitelistedSpectates = settings.Whitelist.Split(',');

        if (!string.IsNullOrEmpty(settings.Whitelist))
        {
            nrOfPlayers += whitelistedSpectates.Count();
        }

        return await server.Remote.GetGuestListAsync(nrOfPlayers, 0);
    }
}
