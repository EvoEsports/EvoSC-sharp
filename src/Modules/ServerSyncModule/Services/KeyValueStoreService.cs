using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;
using GBX.NET.Extensions;
using Microsoft.Extensions.Logging;
using NATS.Client.KeyValue;
using NATS.Client.KeyValueStore;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class KeyValueStoreService(INatsConnectionService nats, ILogger<KeyValueStoreService> logger) : IKeyValueStoreService
{
    public async ValueTask<ulong> CreateEntryAsync<T>(string key, T value)
    {
        logger.LogInformation("Creating key value entry from store with key: {Key}", key);
        return await nats.NatsKV.CreateAsync(key, value);
    }

    public async ValueTask<ulong> CreateOrUpdateEntryAsync<T>(string key, T value)
    {
        NatsKVEntry<T> entryExists = await nats.NatsKV.GetEntryAsync<T>(key);

        if (entryExists.Error != null)
        {
            logger.LogInformation("Creating key value entry from store with key: {Key}", key);
            return await nats.NatsKV.C(key, value);
        }
        
        logger.LogInformation("Updating existing key value entry with key: {Key}", key);
        return await nats.NatsKV.UpdateAsync(key, value, entryExists.Revision);
    }
    
    public async ValueTask<ulong> UpdateEntryAsync<T>(string key, T value, ulong revision)
    {
        logger.LogInformation("Updating key value entry from store with key: {Key}", key);
        return await nats.NatsKV.UpdateAsync(key, value, revision);
    }
    
    public async ValueTask<NatsKVEntry<T>> GetEntryAsync<T>(string key, ulong revision)
    {
        logger.LogInformation("Getting key value entry from store with key: {Key}", key);
        return await nats.NatsKV.GetEntryAsync<T>(key, revision);
    }
    
    public async ValueTask DeleteEntryAsync(string key, ulong revision)
    {
        logger.LogInformation("Deleting key value entry from store with key: {Key}", key);
        await nats.NatsKV.DeleteAsync(key);
    }
}
