using System.Collections.Concurrent;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.ModuleManagerModule.Interfaces;

namespace EvoSC.Modules.Official.ModuleManagerModule.Services;

/// <summary>
/// Registered as a singleton deliberately: <see cref="ModuleManagerService"/> is transient (a new
/// instance per call), so the lock table has to live outside it to actually serialize operations
/// across separate calls, such as two manialink clicks resolving two different service instances.
/// </summary>
[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class ModuleOperationLocks : IModuleOperationLocks
{
    private readonly ConcurrentDictionary<Guid, SemaphoreSlim> _locks = new();

    public async Task RunExclusiveAsync(Guid loadId, Func<Task> operation)
    {
        var moduleLock = _locks.GetOrAdd(loadId, _ => new SemaphoreSlim(1, 1));
        await moduleLock.WaitAsync();

        try
        {
            await operation();
        }
        finally
        {
            // Kept in the dictionary rather than removed after use: removing it here would race a
            // waiter that already fetched the same SemaphoreSlim but hasn't resumed yet, letting it
            // run concurrently with a new caller that creates a replacement lock for the same load
            // ID - exactly the race this class exists to prevent.
            moduleLock.Release();
        }
    }
}
