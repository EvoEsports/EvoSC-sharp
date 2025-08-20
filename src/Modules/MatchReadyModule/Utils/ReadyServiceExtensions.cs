using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;

namespace EvoSC.Modules.Official.MatchReadyModule.Utils;

public static class ReadyServiceExtensions
{
    /// <summary>
    /// Set a player's status as ready.
    /// </summary>
    /// <param name="readyService"></param>
    /// <param name="player">The player to be set as ready.</param>
    /// <returns></returns>
    public static Task SetPlayerReadyAsync(this IReadyService readyService, IPlayer player) => 
        readyService.SetPlayerReadyStatusAsync(player, true);
    
    /// <summary>
    /// Remove a player's ready status.
    /// </summary>
    /// <param name="readyService"></param>
    /// <param name="player">Player to remove the ready status.</param>
    /// <returns></returns>
    public static Task RemovePlayerReadyAsync(this IReadyService readyService, IPlayer player) => 
        readyService.SetPlayerReadyStatusAsync(player, false);
}
