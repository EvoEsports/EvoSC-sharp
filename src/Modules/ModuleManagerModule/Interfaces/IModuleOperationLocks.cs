namespace EvoSC.Modules.Official.ModuleManagerModule.Interfaces;

/// <summary>
/// Serializes lifecycle operations (enable/disable/reload/unload/uninstall) per module load ID, so
/// that triggering one again (e.g. a repeated manialink click) before the first finishes waits for
/// it instead of racing it against shared ControllerManager/ChatCommandManager/container state.
/// </summary>
public interface IModuleOperationLocks
{
    /// <summary>
    /// Runs <paramref name="operation"/> while holding the lock for <paramref name="loadId"/>,
    /// waiting for any other in-flight operation on the same module to finish first.
    /// </summary>
    Task RunExclusiveAsync(Guid loadId, Func<Task> operation);
}
