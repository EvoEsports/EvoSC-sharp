using EvoSC.Common.Database.Repository;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Services.Attributes;
using EvoSC.Modules.Official.RankingModule.Database.Models;
using EvoSC.Modules.Official.RankingModule.Interfaces;
using LinqToDB;

namespace EvoSC.Modules.Official.RankingModule.Database.Repository;

[Service]
public class RankingRepository(IDbConnectionFactory dbConnFactory) : DbRepository(dbConnFactory), IRankingRepository
{
    public async Task<IReadOnlyList<DbPlayerRankingStats>> GetAllStatsAsync() =>
        await AsyncExtensions.ToListAsync(Table<DbPlayerRankingStats>().LoadWith(s => s.DbPlayer));

    public async Task AddPlaytimeAsync(long playerId, long seconds)
    {
        var stats = await GetOrCreateStatsAsync(playerId);
        stats.PlaytimeSeconds += seconds;
        await Database.UpdateAsync(stats);
    }

    public async Task AddMapResultAsync(long playerId, bool won)
    {
        var stats = await GetOrCreateStatsAsync(playerId);
        stats.MapsPlayed++;

        if (won)
        {
            stats.MapsWon++;
        }

        await Database.UpdateAsync(stats);
    }

    private async Task<DbPlayerRankingStats> GetOrCreateStatsAsync(long playerId)
    {
        var stats = await AsyncExtensions.FirstOrDefaultAsync(Table<DbPlayerRankingStats>(), s => s.PlayerId == playerId);

        if (stats != null)
        {
            return stats;
        }

        stats = new DbPlayerRankingStats { PlayerId = playerId };
        stats.Id = Convert.ToInt64(await Database.InsertWithIdentityAsync(stats));
        return stats;
    }
}
