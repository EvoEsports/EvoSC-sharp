using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Models;

/// <inheritdoc cref="IKnownInternalModules"/>
public class KnownInternalModules(IEnumerable<string> moduleIds) : IKnownInternalModules
{
    private readonly HashSet<string> _moduleIds = new(moduleIds, StringComparer.Ordinal);

    public bool Contains(string moduleId) => _moduleIds.Contains(moduleId);
}
