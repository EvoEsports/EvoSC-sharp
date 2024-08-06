using System.Globalization;
using EvoSC.Common.Database.Models.Permissions;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using GbxRemoteNet.Structs;
using LinqToDB;

namespace EvoSC.Common.Database.Repository.Players;

public class PlayerRepository(IDbConnectionFactory dbConnFactory, IPermissionRepository permissionRepository) : DbRepository(dbConnFactory), IPlayerRepository
{
    public async Task<DbPlayer?> GetPlayerByAccountIdAsync(string accountId)
    {
        var player = await Table<DbPlayer>()
            .LoadWith(p => p.DbSettings)
            .SingleOrDefaultAsync(t => t.AccountId == accountId);

        if (player == null)
        {
            return null;
        }

        var groups = await permissionRepository.GetGroupsAsync(player.Id);
        player.Groups = groups;

        var displayGroup = await (
            from g in Table<DbGroup>()
            join ug in Table<DbUserGroup>() on g.Id equals ug.GroupId
            where ug.UserId == player.Id && ug.Display
            select g
        ).FirstOrDefaultAsync();

        player.DisplayGroup = displayGroup;

        return player;
    }

    public async Task<IPlayer> AddPlayerAsync(string accountId, TmPlayerDetailedInfo playerInfo)
    {
        var player = new DbPlayer
        {
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            AccountId = accountId.ToLower(CultureInfo.InvariantCulture),
            NickName = playerInfo.NickName ?? accountId,
            UbisoftName = playerInfo.NickName ?? accountId,
            Zone = playerInfo.Path ?? "World",
            Groups = Array.Empty<IGroup>()
        };

        var id = await Database.InsertWithIdentityAsync(player);
        player.Id =  Convert.ToInt64(id);

        var playerSettings = new DbPlayerSettings
        {
            PlayerId = player.Id, 
            DisplayLanguage = "en"
        };

        await Database.InsertAsync(playerSettings);

        return player;
    }

    public Task UpdateLastVisitAsync(IPlayer player) => Table<DbPlayer>()
        .Where(t => t.Id == player.Id)
        .Set(t => t.LastVisit, DateTime.UtcNow)
        .UpdateAsync();

    public Task UpdateNicknameAsync(IPlayer player, string newNickname) => Table<DbPlayer>()
        .Where(t => t.Id == player.Id)
        .Set(t => t.NickName, newNickname)
        .UpdateAsync();
}
