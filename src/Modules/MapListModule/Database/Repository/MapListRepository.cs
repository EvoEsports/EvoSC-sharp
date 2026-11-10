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

    public async Task<bool> ToggleFavoriteAsync(long playerId, long mapId)
    {
        var deleted = await Table<DbMapFavorite>()
            .Where(f => f.PlayerId == playerId && f.MapId == mapId)
            .DeleteAsync();

        if (deleted > 0)
        {
            return false;
        }

        await Database.InsertAsync(new DbMapFavorite { PlayerId = playerId, MapId = mapId });
        return true;
    }

    public async Task<IReadOnlyList<DbMapTag>> GetAllTagsAsync() =>
        await AsyncExtensions.ToListAsync(Queryable.OrderBy(Table<DbMapTag>(), t => t.Name));

    public async Task<Dictionary<long, List<DbMapTag>>> GetTagsByMapAsync()
    {
        var tags = await GetAllTagsAsync();
        var links = await AsyncExtensions.ToListAsync(Table<DbMapTagMap>());
        var tagsById = tags.ToDictionary(t => t.Id);

        return links
            .Where(l => tagsById.ContainsKey(l.MapTagId))
            .GroupBy(l => l.MapId)
            .ToDictionary(g => g.Key, g => g.Select(l => tagsById[l.MapTagId]).OrderBy(t => t.Name).ToList());
    }

    public async Task AddTagToMapAsync(long mapId, string tagName)
    {
        var tag = await AsyncExtensions.FirstOrDefaultAsync(Table<DbMapTag>(), t => t.Name == tagName);

        if (tag == null)
        {
            tag = new DbMapTag { Name = tagName };
            tag.Id = Convert.ToInt32(await Database.InsertWithIdentityAsync(tag));
        }

        var exists = await AsyncExtensions.AnyAsync(Table<DbMapTagMap>(),
            l => l.MapId == mapId && l.MapTagId == tag.Id);

        if (!exists)
        {
            await Database.InsertAsync(new DbMapTagMap { MapId = mapId, MapTagId = tag.Id });
        }
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
