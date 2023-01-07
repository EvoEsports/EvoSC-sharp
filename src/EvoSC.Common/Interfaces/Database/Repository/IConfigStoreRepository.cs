using EvoSC.Common.Database.Models.Config;

namespace EvoSC.Common.Interfaces.Database.Repository;

public interface IConfigStoreRepository
{
    public Task<DbConfigOption?> GetConfigOptionsByKeyAsync(string keyname);

    public Task AddConfigOptionAsync(DbConfigOption option);

    public Task UpdateConfigOptionAsync(DbConfigOption option);
}
