using System.Runtime.CompilerServices;

namespace EvoSC.Modules.Util;

internal static class CollectibleLoadContext
{
    private const int MaxAttempts = 10;

    /// <summary>
    /// Waits until <paramref name="loadContext"/> has been collected. Returns as soon as it is
    /// gone, or after <paramref name="attempts"/> collection passes.
    /// </summary>
    // A collectible load context is only torn down by the garbage collector and the BCL offers no
    // way to ask for that collection, so an unload has to request it to learn whether the unload
    // completed. Must stay uninlinable so the caller's references don't keep the context alive.
#pragma warning disable S1215
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static void WaitForUnload(WeakReference? loadContext, int attempts = MaxAttempts)
    {
        if (loadContext == null)
        {
            return;
        }

        for (var i = 0; i < attempts && loadContext.IsAlive; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
#pragma warning restore S1215
}
