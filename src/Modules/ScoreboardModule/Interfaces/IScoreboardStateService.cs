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
}
