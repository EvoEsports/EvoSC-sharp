using EvoSC.Common.Database.Models.Permissions;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Database.Repository;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.GroupsModule.Database.Models;
using EvoSC.Modules.Official.GroupsModule.Interfaces;
using LinqToDB;

namespace EvoSC.Modules.Official.GroupsModule.Database.Repository;

[Service]
public class GroupsRepository(IDbConnectionFactory dbConnFactory) : DbRepository(dbConnFactory), IGroupsRepository
{
    public async Task<IReadOnlyList<DbGroup>> GetGroupsAsync() =>
        await AsyncExtensions.ToListAsync(Queryable.OrderBy(Table<DbGroup>(), g => g.Id));

    public async Task<IReadOnlyList<DbPermission>> GetPermissionsAsync() =>
        await AsyncExtensions.ToListAsync(Queryable.OrderBy(Table<DbPermission>(), p => p.Name));

    public async Task<IReadOnlySet<int>> GetGroupPermissionIdsAsync(int groupId)
    {
        var ids = await AsyncExtensions.ToListAsync(
            Queryable.Select(Queryable.Where(Table<DbGroupPermission>(), gp => gp.GroupId == groupId),
                gp => gp.PermissionId));

        return ids.ToHashSet();
    }

    public async Task<IReadOnlyDictionary<int, int>> GetPermissionCountsAsync()
    {
        var groupIds = await AsyncExtensions.ToListAsync(Queryable.Select(Table<DbGroupPermission>(), gp => gp.GroupId));
        return groupIds.GroupBy(id => id).ToDictionary(g => g.Key, g => g.Count());
    }

    public async Task<IReadOnlyDictionary<int, int>> GetMemberCountsAsync()
    {
        var groupIds = await AsyncExtensions.ToListAsync(Queryable.Select(Table<DbUserGroup>(), ug => ug.GroupId));
        return groupIds.GroupBy(id => id).ToDictionary(g => g.Key, g => g.Count());
    }

    public async Task<IReadOnlySet<int>> GetPlayerGroupIdsAsync(long playerId)
    {
        var ids = await AsyncExtensions.ToListAsync(
            Queryable.Select(Queryable.Where(Table<DbUserGroup>(), ug => ug.UserId == playerId), ug => ug.GroupId));

        return ids.ToHashSet();
    }

    public async Task<(IReadOnlyList<DbPlayer> Players, int Total)> GetPlayersAsync(string? search, int skip, int take)
    {
        // the Nadeo placeholder is not a real player, so it should not be given any groups
        var nadeoAccountId = PlayerUtils.NadeoPlayer.AccountId;
        var query = Queryable.Where(Table<DbPlayer>(), p => p.AccountId != nadeoAccountId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = Queryable.Where(query, p => p.NickName.Contains(term) || p.UbisoftName.Contains(term));
        }

        var total = await AsyncExtensions.CountAsync(query);
        var players = await AsyncExtensions.ToListAsync(
            Queryable.Take(Queryable.Skip(Queryable.OrderBy(query, p => p.UbisoftName), skip), take));

        return (players, total);
    }

    public async Task<IReadOnlyDictionary<long, IReadOnlyList<int>>> GetPlayersGroupIdsAsync(
        IEnumerable<long> playerIds)
    {
        var ids = playerIds.ToList();

        var userGroups = await AsyncExtensions.ToListAsync(
            Queryable.Where(Table<DbUserGroup>(), ug => ids.Contains(ug.UserId)));

        return userGroups
            .GroupBy(ug => ug.UserId)
            .ToDictionary(g => g.Key, g => (IReadOnlyList<int>)g.Select(ug => ug.GroupId).ToList());
    }

    public async Task<int> CountUnrestrictedPlayersAsync()
    {
        var query =
            from ug in Table<DbUserGroup>()
            join g in Table<DbGroup>() on ug.GroupId equals g.Id
            where g.Unrestricted
            select ug.UserId;

        return (await AsyncExtensions.ToListAsync(query)).Distinct().Count();
    }

    public async Task<IReadOnlySet<int>> GetScoreboardGroupIdsAsync()
    {
        var ids = await AsyncExtensions.ToListAsync(Queryable.Select(Table<DbScoreboardGroup>(), g => g.GroupId));
        return ids.ToHashSet();
    }

    public async Task SetScoreboardVisibleAsync(int groupId, bool visible)
    {
        await Table<DbScoreboardGroup>().DeleteAsync(g => g.GroupId == groupId);

        if (visible)
        {
            await Database.InsertAsync(new DbScoreboardGroup { GroupId = groupId });
        }
    }
}
