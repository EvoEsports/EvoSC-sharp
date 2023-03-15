namespace EvoSC.Manialinks.Interfaces.Models;

public interface IManialinkActionContext
{
    public IManialinkAction Action { get; }
    public object? EntryModel { get; }
}
