using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Database.Repository;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MotdModule.Database.Models;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using LinqToDB;

namespace EvoSC.Modules.Official.MotdModule.Database.Repositories;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class MotdRepository : DbRepository, IMotdRepository
{
    public MotdRepository(IDbConnectionFactory dbConnFactory) : base(dbConnFactory)
    {
    }

    public Task<MotdEntry?> GetEntryAsync(IPlayer player)
        => Table<MotdEntry>()
            .LoadWith(r => r.DbPlayer)
            .SingleOrDefaultAsync(r => r.PlayerId == player.Id);
    
    public async Task<MotdEntry> InsertOrUpdateEntryAsync(IPlayer player, bool hidden)
    {
        var record = new MotdEntry
        {
            PlayerId = player.Id,
            DbPlayer = new DbPlayer(player),
            Hidden = hidden
        };

        var playerEntry = await GetEntryAsync(player);
        if (playerEntry is not null)
        {
            playerEntry.Hidden = record.Hidden;
            await Database.UpdateAsync(playerEntry);
        }
        else
        {
            await Database.InsertAsync(record);
        }
        return record;
    }
}
