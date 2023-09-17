using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.WorldRecordModule.Models;

namespace EvoSC.Modules.Official.WorldRecordModule.Interfaces;

public interface IWorldRecordService
{
    /// <summary>
    /// Trigger a fetch for records from trackmania.io
    /// </summary>
    /// <param name="map">The map to load the records for.</param>
    /// <returns></returns>
    public Task FetchRecord(IMap map);
    
    /// <summary>
    /// Overwrite the current record with the given one.
    /// </summary>
    /// <param name="newRecord">The record to set.</param>
    /// <returns></returns>
    public Task OverwriteRecord(WorldRecord newRecord);
    
    /// <summary>
    /// Gets the currently loaded world record or null.
    /// </summary>
    /// <returns>WR or null.</returns>
    public Task<WorldRecord?> GetRecord();

    /// <summary>
    /// Goes through scores and checks whether the world record has been beaten.
    /// If yes update record locally.
    /// </summary>
    /// <param name="scoresEventArgs">The event args of the round scores.</param>
    /// <returns></returns>
    Task DetectNewWorldRecordThroughScores(ScoresEventArgs scoresEventArgs);
}
