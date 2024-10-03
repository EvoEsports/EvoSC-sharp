using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Util;
using EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;
using EvoSC.Modules.EvoEsports.ToornamentModule.Permissions;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Controllers;

[Controller]
public class MatchConfigurationController(IMatchService matchService, IServerClient server, ILogger<MatchConfigurationController> logger) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("toornament_setup", "Setup the server for a match from toornament.", ToornamentPermissions.SetupMatch)]
    public async Task ToornamentSetupAsync()
    {
        try
        {
            await server.Chat.InfoMessageAsync($"Setting up a new Toornament match, please wait ...");
            await matchService.ShowSetupScreenAsync(Context.Player, string.Empty, string.Empty);
        }
        catch (InvalidOperationException ex)
        {
            var chatMessage = FormattingUtils.FormatPlayerChatMessage(Context.Player, $"(Toornament) {ex.Message}", false);
            await server.Chat.ErrorMessageAsync(chatMessage);
        }
        catch (Exception)
        {
            var chatMessage = FormattingUtils.FormatPlayerChatMessage(Context.Player, $"(Toornament) An unknown error occured, check console.", false);
            await server.Chat.ErrorMessageAsync(chatMessage);
            throw;
        }
    }

    [ChatCommand("toornament_startmatch", "Start a toornament match.", ToornamentPermissions.StartMatch)]
    [CommandAlias("/toornament_start")]
    public async Task ToornamentStartMatchAsync()
    {
        try
        {
            await matchService.StartMatchAsync();
        }
        catch (Exception ex)
        {
            var chatMessage = FormattingUtils.FormatPlayerChatMessage(Context.Player, $"Failed to start match: {ex.Message}", false);
            await server.Chat.ErrorMessageAsync(chatMessage);
            throw;
        }
    }

    [ChatCommand("servername", "Set the current server name.", ToornamentPermissions.ServerName)]
    public Task SetServerNameAsync(string name) => matchService.SetServerNameAsync(name);
}

