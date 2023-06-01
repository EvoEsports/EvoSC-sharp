using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks.Models;

public class ManialinkActionContext : IManialinkActionContext
{
    public required IManialinkAction Action { get; init; }
    public object? EntryModel { get; init; }
}
