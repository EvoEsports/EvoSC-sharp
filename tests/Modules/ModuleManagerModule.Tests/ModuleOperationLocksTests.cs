using EvoSC.Modules.Official.ModuleManagerModule.Services;

namespace EvoSC.Modules.Official.ModuleManagerModule.Tests;

/// <summary>
/// Regression tests for the race that let two manialink clicks on the same module run their
/// lifecycle operations concurrently (see ModuleManagerService), corrupting shared
/// ControllerManager/ChatCommandManager state.
/// </summary>
public class ModuleOperationLocksTests
{
    [Fact]
    public async Task RunExclusiveAsync_Serializes_Operations_For_The_Same_Load_Id()
    {
        var locks = new ModuleOperationLocks();
        var loadId = Guid.NewGuid();
        var gate = new TaskCompletionSource();
        var events = new List<string>();

        // Starts synchronously up to the await on `gate`, so by the time this call returns,
        // "first-start" is already recorded and the lock is held.
        var first = locks.RunExclusiveAsync(loadId, async () =>
        {
            events.Add("first-start");
            await gate.Task;
            events.Add("first-end");
        });

        var second = locks.RunExclusiveAsync(loadId, () =>
        {
            events.Add("second-start");
            return Task.CompletedTask;
        });

        // The second operation must not run while the first still holds the lock.
        Assert.DoesNotContain("second-start", events);

        gate.SetResult();
        await Task.WhenAll(first, second);

        Assert.Equal(new[] {"first-start", "first-end", "second-start"}, events);
    }

    [Fact]
    public async Task RunExclusiveAsync_Does_Not_Serialize_Different_Load_Ids()
    {
        var locks = new ModuleOperationLocks();
        var gate = new TaskCompletionSource();
        var secondStarted = new TaskCompletionSource();

        var first = locks.RunExclusiveAsync(Guid.NewGuid(), () => gate.Task);
        var second = locks.RunExclusiveAsync(Guid.NewGuid(), () =>
        {
            secondStarted.SetResult();
            return Task.CompletedTask;
        });

        var completed = await Task.WhenAny(secondStarted.Task, Task.Delay(TimeSpan.FromSeconds(5)));
        Assert.Same(secondStarted.Task, completed);

        gate.SetResult();
        await Task.WhenAll(first, second);
    }
}
