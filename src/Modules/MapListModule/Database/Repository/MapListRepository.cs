using EvoSC.Common.Database.Repository;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Services.Attributes;
using EvoSC.Modules.Official.MapListModule.Database.Models;
using EvoSC.Modules.Official.MapListModule.Interfaces.Database;
using LinqToDB;

namespace EvoSC.Modules.Official.MapListModule.Database.Repository;

[Service]
public class MapListRepository(IDbConnectionFactory dbConnFactory) : DbRepository(dbConnFactory), IMapListRepository
{
    public async Task<HashSet<long>> GetFavoriteMapIdsAsync(long playerId)
    {
        var ids = await AsyncExtensions.ToListAsync(
            Queryable.Select(Queryable.Where(Table<DbMapFavorite>(), f => f.PlayerId == playerId), f => f.MapId));
        return ids.ToHashSet();
    }

    public async Task SetFavoriteAsync(long playerId, long mapId, bool favorite)
    {
        if (!favorite)
        {
            await Table<DbMapFavorite>()
                .Where(f => f.PlayerId == playerId && f.MapId == mapId)
                .DeleteAsync();
            return;
        }

        // the unique (PlayerId, MapId) index makes this safe to repeat
        await Table<DbMapFavorite>()
            .Merge()
            .Using(new[] { new DbMapFavorite { PlayerId = playerId, MapId = mapId } })
            .On((t, src) => t.PlayerId == src.PlayerId && t.MapId == src.MapId)
            .InsertWhenNotMatched()
            .MergeAsync();
    }

    public async Task<IReadOnlyList<DbMapTag>> GetAllTagsAsync() =>
        await AsyncExtensions.ToListAsync(Queryable.OrderBy(Table<DbMapTag>(), t => t.Name));

    public async Task<Dictionary<long, List<DbMapTag>>> GetTagsByMapAsync()
    {
        var links = await AsyncExtensions.ToListAsync(
            from link in Table<DbMapTagMap>()
            join tag in Table<DbMapTag>() on link.MapTagId equals tag.Id
            orderby tag.Name
            select new { link.MapId, Tag = tag });

        return links
            .GroupBy(l => l.MapId)
            .ToDictionary(g => g.Key, g => g.Select(l => l.Tag).ToList());
    }

    public async Task AddTagToMapAsync(long mapId, string tagName)
    {
        // tag names are unique, so adding an existing name does nothing
        await Table<DbMapTag>()
            .Merge()
            .Using(new[] { new DbMapTag { Name = tagName } })
            .On((t, src) => t.Name == src.Name)
            .InsertWhenNotMatched()
            .MergeAsync();

        // link the tag, unless the map already has it
        await Queryable.Where(Table<DbMapTag>(), t => t.Name == tagName &&
                                                      !Table<DbMapTagMap>().Any(l => l.MapId == mapId && l.MapTagId == t.Id))
            .InsertAsync(Table<DbMapTagMap>(), t => new DbMapTagMap { MapTagId = t.Id, MapId = mapId });
    }

    public async Task<bool> RemoveTagFromMapAsync(long mapId, string tagName)
    {
        var tag = await AsyncExtensions.FirstOrDefaultAsync(Table<DbMapTag>(), t => t.Name == tagName);

        if (tag == null)
        {
            return false;
        }

        var removed = await Table<DbMapTagMap>()
            .Where(l => l.MapId == mapId && l.MapTagId == tag.Id)
            .DeleteAsync();

        var stillUsed = await AsyncExtensions.AnyAsync(Table<DbMapTagMap>(), l => l.MapTagId == tag.Id);
        if (!stillUsed)
        {
            await Database.DeleteAsync(tag);
        }

        return removed > 0;
    }
}
