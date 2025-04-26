using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.WorldRecordModule.Models;

namespace EvoSC.Modules.Official.WorldRecordModule.Interfaces;

public interface IWorldRecordService
{
    /// <summary>
    /// Trigger a fetch for records from trackmania.io.
    /// If an error occurs during the API fetch, the method will log the error and use AT instead.
    /// </summary>
    /// <param name="mapUid">The UID of the map to load records from.</param>
    /// <returns></returns>
    public Task FetchRecordAsync(string mapUid);

    /// <summary>
    /// Clears the currently loaded world record.
    /// </summary>
    /// <returns></returns>
    public Task ClearRecordAsync();

    /// <summary>
    /// Gets the currently loaded world record or null.
    /// </summary>
    /// <returns>WR or null.</returns>
    public Task<WorldRecord?> GetRecordAsync();

    /// <summary>
    /// Goes through scores and checks whether the world record has been beaten.
    /// If yes update record locally.
    /// </summary>
    /// <param name="scoresEventArgs">The event args of the round scores.</param>
    /// <returns></returns>
    Task DetectNewWorldRecordThroughScoresAsync(ScoresEventArgs scoresEventArgs);
}
