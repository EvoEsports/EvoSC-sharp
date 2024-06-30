using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks.Models;

public class PersistentManialink : IPersistentManialink
{
    public required string Name { get; init; }
    public required PersistentManialinkType Type { get; init; }
    public string CompiledOutput { get; init; }
    public Func<Task<IDictionary<string, object?>>>? DynamicDataCallbackAsync { get; init; }
}
