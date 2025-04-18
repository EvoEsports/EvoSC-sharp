using NATS.Client.KeyValue;
using NATS.Client.KeyValueStore;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;

public interface IKeyValueStoreService
{
    /// <summary>
    /// Creates a new key value entry.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns>Revision number of the key</returns>
    ValueTask<ulong> CreateEntryAsync<T>(string key, T value);

    /// <summary>
    /// Create a new key value entry if one does not exist, if it does it updates.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns>Revision number of the key</returns>
    ValueTask<ulong> CreateOrUpdateEntryAsync<T>(string key, T value);
    
    /// <summary>
    /// Updates an existing key value entry.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="revision"></param>
    ValueTask<ulong> UpdateEntryAsync<T>(string key, T value, ulong revision);
    
    /// <summary>
    /// Gets an existing key value entry.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="revision"></param>
    /// <returns>Key and value of the entry</returns>
    ValueTask<NatsKVEntry<T>> GetEntryAsync<T>(string key, ulong revision);
    
    /// <summary>
    /// Deletes an existing key value entry.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="revision"></param>
    ValueTask DeleteEntryAsync(string key, ulong revision);
}
