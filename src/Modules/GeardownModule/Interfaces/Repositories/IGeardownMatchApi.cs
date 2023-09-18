using System.Collections.Generic;
using System.Threading.Tasks;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Evo.GeardownModule.Models;
using EvoSC.Modules.Evo.GeardownModule.Models.API;

namespace EvoSC.Modules.Evo.GeardownModule.Interfaces.Repositories;

public interface IGeardownMatchApi
{
    /// <summary>
    /// Assign a server to a match.
    /// </summary>
    /// <param name="matchId">ID of the match.</param>
    /// <param name="name">Name of the server.</param>
    /// <param name="serverId">Unique ID of the server. (typically account uid)</param>
    /// <param name="serverPassword">The password required to join the server.</param>
    /// <returns></returns>
    public Task<GdMatchToken?> AssignServerAsync(int matchId, string name, string serverId, string? serverPassword);
    
    /// <summary>
    /// Get details about a match from a match token.
    /// </summary>
    /// <param name="matchToken">Token corresponding to the match.</param>
    /// <returns></returns>
    public Task<GdMatch?> GetMatchDataByTokenAsync(string matchToken);
    
    /// <summary>
    /// Notify the end of match.
    /// </summary>
    /// <param name="matchToken">Token of the match.</param>
    /// <returns></returns>
    public Task OnEndMatchAsync(string matchToken);
    
    /// <summary>
    /// Notify start of match.
    /// </summary>
    /// <param name="matchToken">Token of the match.</param>
    /// <returns></returns>
    public Task OnStartMatchAsync(string matchToken);
    
    /// <summary>
    /// Add results to a match.
    /// </summary>
    /// <param name="matchId">ID of the match.</param>
    /// <param name="results">Results containing an array of account ID and score pairs.</param>
    /// <returns></returns>
    public Task AddResultsAsync(int matchId, IEnumerable<GdResult> results);
}
