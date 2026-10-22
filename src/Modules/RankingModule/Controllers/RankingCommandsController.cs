using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Util.TextFormatting;
using EvoSC.Modules.Official.RankingModule.Interfaces;
using EvoSC.Modules.Official.RankingModule.Models;
using EvoSC.Modules.Official.RankingModule.Models;

namespace EvoSC.Modules.Official.RankingModule.Controllers;

[Controller]
public class RankingCommandsController(IRankingService rankingService, IThemeManager themes)
    : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("rank", "Show your rank on the server, based on playtime and win rate.")]
    public async Task RankAsync()
    {
        var rank = await rankingService.GetPlayerRankAsync(Context.Player.AccountId);

        if (rank == null)
        {
            await Context.Chat.InfoMessageAsync($"{new TextColor(themes.Theme.Chat_Info)}You are not ranked yet, play a few maps first.", Context.Player);
            return;
        }

        await Context.Chat.InfoMessageAsync(
            $"{new TextColor(themes.Theme.Chat_Info)}You are rank {PlayerRank.Highlight($"{rank.Rank}/{rank.Total}")} - {rank.Summary()}", Context.Player);
    }

    [ChatCommand("ranks", "Show the best ranked players on the server.")]
    public Task RanksAsync() => rankingService.ShowRankingAsync(Context.Player);
}
