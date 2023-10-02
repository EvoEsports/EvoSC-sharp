namespace EvoSC.Modules.Official.LiveRankingModule.Models;

/// <summary>
/// The live ranking position of a player.
/// </summary>
/// <param name="accountId">Account Id of the player.</param>
/// <param name="cpTime">The checkpoint time of the player.</param>
/// <param name="cpIndex">The checkpoint index that was driven through.</param>
/// <param name="isDNF">Whether the player has given up or not.</param>
/// <param name="isFinish">Whether the player has finished.</param>
public record LiveRankingPosition(string accountId, int cpTime, int cpIndex, bool isDNF, bool isFinish);
