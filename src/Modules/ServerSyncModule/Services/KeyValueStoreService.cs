using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;
using Microsoft.Extensions.Logging;
using NATS.Client.KeyValue;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class KeyValueStoreService(INatsConnectionService nats, ILogger<KeyValueStoreService> logger) : IKeyValueStoreService
{
    public ulong CreateEntry(string key, byte[] value)
    {
        logger.LogInformation("Creating key value entry from store with key: {Key}", key);
        return nats.KeyValue.Create(key, value);
    }

    public ulong CreateOrUpdateEntry(string key, byte[] value)
    {
        var entryExists = nats.KeyValue.Get(key);

        if (entryExists is null)
        {
            logger.LogInformation("Creating key value entry from store with key: {Key}", key);
            return nats.KeyValue.Create(key, value);
        }
        
        logger.LogInformation("Updating existing key value entry with key: {Key}", key);
        return nats.KeyValue.Update(entryExists.Key, value, entryExists.Revision);
    }
    
    public void UpdateEntry(string key, byte[] value, ulong revision)
    {
        logger.LogInformation("Updating key value entry from store with key: {Key}", key);
        nats.KeyValue.Update(key, value, revision);
    }
    
    public KeyValueEntry GetEntry(string key, ulong revision)
    {
        logger.LogInformation("Getting key value entry from store with key: {Key}", key);
        return nats.KeyValue.Get(key, revision);
    }
    
    public void DeleteEntry(string key, ulong revision)
    {
        logger.LogInformation("Deleting key value entry from store with key: {Key}", key);
        nats.KeyValue.Delete(key, revision);
    }
}
