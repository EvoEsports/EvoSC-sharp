using System.Data.Common;
using System.Globalization;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using GbxRemoteNet.Structs;
using Npgsql;
using RepoDb;

namespace EvoSC.Common.Database.Repository.Players;

public class PlayerRepository : EvoScDbRepository<DbPlayer>, IPlayerRepository
{
    
    public PlayerRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }
    
    public async Task<IPlayer?> GetPlayerByAccountIdAsync(string accountId)
    {
        var player = await Database.QueryAsync<DbPlayer>(e => e.AccountId == accountId);
        return player.FirstOrDefault();
    }

    public async Task<IPlayer> AddPlayerAsync(string accountId, TmPlayerDetailedInfo playerInfo)
    {
        var dbPlayer = new DbPlayer
        {
            AccountId = accountId.ToLower(CultureInfo.InvariantCulture),
            NickName = playerInfo?.NickName ?? accountId,
            UbisoftName = playerInfo?.NickName ?? accountId,
            Zone = playerInfo?.Path ?? "World",
            LastVisit = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var newPlayer = await Database.InsertAsync<DbPlayer>(dbPlayer);
        return (IPlayer)newPlayer;
    }

    public async Task UpdateLastVisitAsync(IPlayer player)
    {
        var dbPlayer = (DbPlayer) player;
        dbPlayer.LastVisit = DateTime.Now;
        var fields = Field.Parse<DbPlayer>(e => new { e.LastVisit });

        await Database.UpdateAsync(player, fields: fields);
    }
}
