namespace EvoSC.Manialinks.Interfaces.Models;

public interface IPersistentManialink
{
    public string Name { get; }
    public PersistentManialinkType Type { get; }
    public string CompiledOutput { get; }
    public Func<Task<IDictionary<string, object?>>>? DynamicDataCallbackAsync { get; }
}
