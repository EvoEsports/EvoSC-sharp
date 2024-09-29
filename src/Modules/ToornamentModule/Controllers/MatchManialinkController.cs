using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Util;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;
using EvoSC.Modules.EvoEsports.ToornamentModule.Permissions;
using EvoSC.Modules.EvoEsports.ToornamentModule.Settings;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Controllers;

[Controller]
public class MatchManialinkController(IMatchService matchService, IServerClient server, IToornamentSettings settings) : ManialinkController
{
    [ManialinkRoute(Route = "SelectMatch/{tournamentId}/{stageId}/{matchId}", Permission = ToornamentPermissions.SetupMatch)]
    public async Task SelectMatchAsync(string tournamentId, string stageId, string matchId)
    {
        await matchService.SetupServerAsync(Context.Player, tournamentId, stageId, matchId);
        var chatMessage = FormattingUtils.FormatPlayerChatMessage(Context.Player, "Match successfully set up!", false);
        await server.Chat.SuccessMessageAsync(chatMessage);
    }

    [ManialinkRoute(Route = "SelectTournament/{tournamentId}", Permission = ToornamentPermissions.SetupMatch)]
    public async Task SelectTournamentAsync(string tournamentId)
    {
        await matchService.ShowSetupScreenAsync(Context.Player, tournamentId, string.Empty);
    }

    [ManialinkRoute(Route = "SelectStage/{tournamentId}/{stageId}", Permission = ToornamentPermissions.SetupMatch)]
    public async Task SelectStageAsync(string tournamentId, string stageId)
    {
        await matchService.ShowSetupScreenAsync(Context.Player, tournamentId, stageId);
    }
}
