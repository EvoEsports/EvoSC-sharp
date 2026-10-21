using EvoSC.Common.Database.Models.Permissions;
using EvoSC.Common.Database.Models.Player;

namespace EvoSC.Modules.Official.GroupsModule.Interfaces;

public interface IGroupsRepository
{
    /// <summary>
    /// Get all groups ordered by ID.
    /// </summary>
    public Task<IReadOnlyList<DbGroup>> GetGroupsAsync();

    /// <summary>
    /// Get all registered permissions ordered by name.
    /// </summary>
    public Task<IReadOnlyList<DbPermission>> GetPermissionsAsync();

    /// <summary>
    /// Get the IDs of the permissions assigned to a group.
    /// </summary>
    public Task<IReadOnlySet<int>> GetGroupPermissionIdsAsync(int groupId);

    /// <summary>
    /// Get the number of permissions assigned to each group, by group ID.
    /// </summary>
    public Task<IReadOnlyDictionary<int, int>> GetPermissionCountsAsync();

    /// <summary>
    /// Get the number of players in each group, by group ID.
    /// </summary>
    public Task<IReadOnlyDictionary<int, int>> GetMemberCountsAsync();

    /// <summary>
    /// Get the IDs of the groups a player is in.
    /// </summary>
    public Task<IReadOnlySet<int>> GetPlayerGroupIdsAsync(long playerId);

    /// <summary>
    /// Get a page of known players ordered by name, optionally filtered by a part of their name.
    /// </summary>
    /// <returns>The players on the page, and the total number of players matching the filter.</returns>
    public Task<(IReadOnlyList<DbPlayer> Players, int Total)> GetPlayersAsync(string? search, int skip, int take);

    /// <summary>
    /// Get the group IDs of each of the given players, by player ID.
    /// </summary>
    public Task<IReadOnlyDictionary<long, IReadOnlyList<int>>> GetPlayersGroupIdsAsync(IEnumerable<long> playerIds);

    /// <summary>
    /// Get the number of distinct players that are in a group with unrestricted permissions.
    /// </summary>
    public Task<int> CountUnrestrictedPlayersAsync();

    /// <summary>
    /// Get the IDs of the groups that are shown as the role of their players on the scoreboard.
    /// </summary>
    public Task<IReadOnlySet<int>> GetScoreboardGroupIdsAsync();

    /// <summary>
    /// Set whether a group is shown as the role of its players on the scoreboard.
    /// </summary>
    public Task SetScoreboardVisibleAsync(int groupId, bool visible);
}
