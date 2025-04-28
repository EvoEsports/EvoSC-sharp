using NATS.Client.KeyValueStore;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Extensions;

// TODO: Change to new extension block when we upgrade to C# 14
public static class NatsKvStoreExtension
{
    public static async ValueTask<ulong> CreateOrUpdateEntryAsync<T>(this INatsKVStore kvStore, string key, T value)
    {
        NatsKVEntry<T> entryExists = await kvStore.GetEntryAsync<T>(key);

        if (entryExists.Error != null)
        {
            return await kvStore.CreateAsync(key, value);
        }

        return await kvStore.UpdateAsync(key, value, entryExists.Revision);
    }
}
