using System.Reflection;
using EvoSC.Manialinks.Vdom.Mount;
using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Runtime;

/// <summary>
/// Loads the static <c>VdomRuntime.ms</c> embedded resource and fills in the handful of
/// per-view tokens it needs (pool sizes, the LocalUser variable name, the ack route) -- see that
/// file's header comment for the full contract. This is text substitution, not code generation:
/// the runtime's actual logic never changes between views.
/// </summary>
public static class RuntimeScriptBuilder
{
    public static string Build(string viewName, IReadOnlyList<PoolBucketRange> layout)
    {
        return LoadTemplate()
            .Replace("{{QuadCount}}", TotalForKind(layout, VKind.Quad).ToString())
            .Replace("{{LabelCount}}", TotalForKind(layout, VKind.Label).ToString())
            .Replace("{{EntryCount}}", TotalForKind(layout, VKind.Entry).ToString())
            .Replace("{{PatchSeqVariableName}}", VdomNaming.PatchSeqVariableName(viewName))
            .Replace("{{PatchOpsVariableName}}", VdomNaming.PatchOpsVariableName(viewName))
            .Replace("{{AckRoutePrefix}}", AckRoutePrefix(viewName));
    }

    /// <summary>
    /// The <c>TriggerPageAction</c> prefix the runtime appends the applied seq to. Must match
    /// <c>VdomAckController</c>'s route exactly (<c>[ManialinkRoute(Route = "VdomAck")]</c>,
    /// method <c>AckAsync(string viewName, int seq)</c> -&gt; route <c>VdomAck/Ack/{viewName}/{seq}</c>).
    /// </summary>
    public static string AckRoutePrefix(string viewName) => $"VdomAck/Ack/{viewName}/";

    private static int TotalForKind(IReadOnlyList<PoolBucketRange> layout, VKind kind) =>
        layout.Where(r => r.Kind == kind).Sum(r => r.Count);

    private static string LoadTemplate()
    {
        var assembly = typeof(RuntimeScriptBuilder).Assembly;
        var resourceName = assembly.GetManifestResourceNames()
            .Single(n => n.EndsWith("VdomRuntime.ms", StringComparison.Ordinal));

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded resource '{resourceName}' not found.");
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}
