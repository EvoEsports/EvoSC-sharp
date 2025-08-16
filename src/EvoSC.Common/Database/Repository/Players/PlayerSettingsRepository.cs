using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using LinqToDB;

namespace EvoSC.Common.Database.Repository.Players;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class PlayerSettingsRepository(DbConnectionFactory dbConnFactory)
    : DbRepository(dbConnFactory), IPlayerSettingsRepository
{
    public Task UpdateHiddenManialinksAsync(IPlayer player, IEnumerable<string> hiddenManialinks) =>
        Table<DbPlayerSettings>()
            .Where(t => t.PlayerId == player.Id)
            .Set(t => t.HiddenManialinks, hiddenManialinks)
            .UpdateAsync();
}
