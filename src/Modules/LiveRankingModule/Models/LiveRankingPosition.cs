namespace EvoSC.Modules.Official.LiveRankingModule.Models;

/// <summary>
/// The live ranking position of a player.
/// </summary>
/// <param name="AccountId">Account Id of the player.</param>
/// <param name="CpTime">The checkpoint time of the player.</param>
/// <param name="CpIndex">The checkpoint index that was driven through.</param>
/// <param name="IsDnf">Whether the player has given up or not.</param>
/// <param name="IsFinish">Whether the player has finished.</param>
public record LiveRankingPosition(string AccountId, int CpTime, int CpIndex, bool IsDnf, bool IsFinish);
