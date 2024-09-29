using NATS.Client.KeyValue;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;

public interface IKeyValueStoreService
{
    /// <summary>
    /// Creates a new key value entry.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns>Revision number of the key</returns>
    ulong CreateEntry(string key, byte[] value);
    
    /// <summary>
    /// Updates an existing key value entry.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="revision"></param>
    void UpdateEntry(string key, byte[] value, ulong revision);
    
    /// <summary>
    /// Gets an existing key value entry.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="revision"></param>
    /// <returns>Key and value of the entry</returns>
    KeyValueEntry GetEntry(string key, ulong revision);
    
    /// <summary>
    /// Deletes an existing key value entry.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="revision"></param>
    void DeleteEntry(string key, ulong revision);
}