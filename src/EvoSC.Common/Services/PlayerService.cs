using System.Data.Common;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSC.Common.Database.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Util;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Services;

public class PlayerService : IPlayerService
{
    private readonly ILogger<PlayerService> _logger;
    private readonly DbConnection _db;

    public PlayerService(ILogger<PlayerService> logger, DbConnection db)
    {
        _logger = logger;
        _db = db;
    }

    public Task<DbPlayer> GetPlayerById(int id) =>
        _db.GetAsync<DbPlayer>(id);

    public async Task<DbPlayer> GetPlayerByLogin(string login)
    {
        var query = "select * from `players` where `AccountId`=@AccountId limit 1";
        var player =
            await _db.QueryAsync<DbPlayer>(query, new
            {
                AccountId = PlayerUtils.ConvertLoginToAccountId(login)
            });

        return player?.FirstOrDefault();
    }

    public async Task<DbPlayer> NewPlayer(string accountId, string ubisoftName, string? zone)
    {
        var newPlayer = new DbPlayer
        {
            AccountId = accountId,
            UbisoftName = ubisoftName,
            Zone = zone,
            NickName = ubisoftName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var id = await _db.InsertAsync(newPlayer);
        return await GetPlayerById(id);
    }

    public Task<DbPlayer> NewPlayer(string login, string ubisoftName) => NewPlayer(login, ubisoftName, null);

    public Task<bool> UpdatePlayer(DbPlayer player)
    {
        player.UpdatedAt = DateTime.UtcNow;
        return _db.UpdateAsync(player);
    }

    public Task<bool> DeletePlayer(DbPlayer player) =>
        _db.DeleteAsync(player);
}
