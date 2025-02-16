namespace EvoSC.Modules.Official.ScoreboardModule.Interfaces;

public interface IScoreboardStateService
{
    /// <summary>
    /// Sets the current round number.
    /// </summary>
    /// <param name="roundNumber"></param>
    /// <returns></returns>
    public Task SetCurrentRoundNumberAsync(int roundNumber);
    
    /// <summary>
    /// Returns the number of the current round.
    /// </summary>
    /// <returns></returns>
    public Task<int> GetCurrentRoundNumberAsync();

    /// <summary>
    /// Sets whether warm is currently ongoing.
    /// </summary>
    /// <param name="isWarmUp"></param>
    /// <returns></returns>
    public Task SetIsWarmUpAsync(bool isWarmUp);
    
    /// <summary>
    /// Returns whether the game mode is in warm up mode.
    /// </summary>
    /// <returns></returns>
    public Task<bool> GetIsWarmUpAsync();
}
