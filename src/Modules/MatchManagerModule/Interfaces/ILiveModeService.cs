namespace EvoSC.Modules.Official.MatchManagerModule.Interfaces;

public interface ILiveModeService
{
    /// <summary>
    /// Get a list of available modes as aliases,
    /// that can be loaded live without configuration.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> GetAvailableModes();
    
    /// <summary>
    /// Load a game mode live with default configuration.
    /// </summary>
    /// <param name="mode">The alias name of the mode to load.</param>
    /// <returns></returns>
    public Task<string> LoadModeAsync(string mode);
}
