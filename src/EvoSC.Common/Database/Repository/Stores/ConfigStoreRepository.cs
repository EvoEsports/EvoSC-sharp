using EvoSC.Common.Database.Models.Config;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Database.Repository;
using LinqToDB;

namespace EvoSC.Common.Database.Repository.Stores;

public class ConfigStoreRepository : DbRepository, IConfigStoreRepository
{
    public ConfigStoreRepository(IDbConnectionFactory dbConnFactory) : base(dbConnFactory)
    {
    }

    public async Task<DbConfigOption?> GetConfigOptionsByKeyAsync(string keyName) => await Table<DbConfigOption>()
        .SingleOrDefaultAsync(t => t.Key == keyName);

    public Task AddConfigOptionAsync(DbConfigOption option) => Database.InsertAsync(option);

    public Task UpdateConfigOptionAsync(DbConfigOption option) => Database.UpdateAsync(option);
}
