using System.Globalization;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using GbxRemoteNet.Structs;
using LinqToDB;
using LinqToDB.Data;

namespace EvoSC.Common.Database.Repository.Players;

public class PlayerRepository : DbRepository, IPlayerRepository
{
    public PlayerRepository(IDbConnectionFactory dbConnFactory) : base(dbConnFactory)
    {
    }

    public async Task<IPlayer?> GetPlayerByAccountIdAsync(string accountId) => await Table<DbPlayer>()
        .SingleAsync(t => t.AccountId == accountId);

    public async Task<IPlayer> AddPlayerAsync(string accountId, TmPlayerDetailedInfo playerInfo)
    {
        var player = new DbPlayer
        {
            LastVisit = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            AccountId = accountId.ToLower(CultureInfo.InvariantCulture),
            NickName = playerInfo.NickName ?? accountId,
            UbisoftName = playerInfo?.NickName ?? accountId,
            Zone = playerInfo?.Path ?? "World"
        };

        await Database.InsertAsync(player);
        return player;
    }

    public Task UpdateLastVisitAsync(IPlayer player) => Table<DbPlayer>()
        .Where(t => t.Id == player.Id)
        .Set(t => t.LastVisit, DateTime.UtcNow)
        .UpdateAsync();
}
