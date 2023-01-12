using EvoSC.Common.Database.Models.Config;

namespace EvoSC.Common.Interfaces.Database.Repository;

public interface IConfigStoreRepository
{
    /// <summary>
    /// Get a config option from it's name.
    /// </summary>
    /// <param name="keyName">The name of the option.</param>
    /// <returns></returns>
    public Task<DbConfigOption?> GetConfigOptionsByKeyAsync(string keyName);

    /// <summary>
    /// Register a new config option in the database.
    /// </summary>
    /// <param name="option">The option to add.</param>
    /// <returns></returns>
    public Task AddConfigOptionAsync(DbConfigOption option);

    /// <summary>
    /// Update the value of an option.
    /// </summary>
    /// <param name="option">The option to update.</param>
    /// <returns></returns>
    public Task UpdateConfigOptionAsync(DbConfigOption option);
}
