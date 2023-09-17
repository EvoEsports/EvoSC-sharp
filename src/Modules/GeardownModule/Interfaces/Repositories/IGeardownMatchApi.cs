using System.Collections.Generic;
using System.Threading.Tasks;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Evo.GeardownModule.Models;
using EvoSC.Modules.Evo.GeardownModule.Models.API;

namespace EvoSC.Modules.Evo.GeardownModule.Interfaces.Repositories;

public interface IGeardownMatchApi
{
    public Task<GdMatchToken?> AssignServerAsync(int matchId, string name, string serverId, string? serverPassword);
    public Task<GdMatch?> GetMatchDataByTokenAsync(string matchToken);
    public Task OnEndMatchAsync(string matchToken);
    public Task OnStartMatchAsync(string matchToken);
    public Task AddResultsAsync(int matchId, IEnumerable<GdResult> results);
}
