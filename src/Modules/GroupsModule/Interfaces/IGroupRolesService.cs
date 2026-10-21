using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.GroupsModule.Models;

namespace EvoSC.Modules.Official.GroupsModule.Interfaces;

public interface IGroupRolesService
{
    /// <summary>
    /// Get the roles of the given players, by account ID. Players without a role are left out.
    /// A player's role is the group with the lowest ID, out of the groups they are in that are set to be shown.
    /// </summary>
    public Task<IReadOnlyDictionary<string, PlayerRole>> GetRolesAsync(IEnumerable<IPlayer> players);
}
