namespace EvoSC.Modules.Official.MatchManagerModule.Interfaces;

public interface IFlowControlService
{
    /// <summary>
    /// End the current round.
    /// </summary>
    /// <returns></returns>
    public Task EndRoundAsync();
    
    /// <summary>
    /// Restart the current match from the start.
    /// </summary>
    /// <returns></returns>
    public Task RestartMatchAsync();
    
    /// <summary>
    /// Skip to the next map in the rotation.
    /// </summary>
    /// <returns></returns>
    public Task SkipMapAsync();
}
