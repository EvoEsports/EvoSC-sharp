using System.Data.Common;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSC.Common.Database.Models;
using EvoSC.Common.Interfaces.Services;
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

    public async Task<DbPlayer> GetPlayerByLogin(string login) =>
        (await _db.QueryAsync("select * from players where Login=@Login limit 1", new {Login = login}))
        .FirstOrDefault();

    public async Task<DbPlayer> NewPlayer(string login, string ubisoftName, string? zone = null)
    {
        var newPlayer = new DbPlayer
        {
            Login = login,
            UbisoftName = ubisoftName,
            Zone = zone,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var id = await _db.InsertAsync(newPlayer);
        return await GetPlayerById(id);
    }

    public Task<bool> UpdatePlayer(DbPlayer player)
    {
        player.UpdatedAt = DateTime.UtcNow;
        return _db.UpdateAsync(player);
    }

    public Task<bool> DeletePlayer(DbPlayer player) =>
        _db.DeleteAsync(player);
}
