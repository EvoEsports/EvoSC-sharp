namespace EvoSC.Manialinks.Interfaces.Models;

public interface IManialinkActionContext
{
    /// <summary>
    /// Information about the current manialink action.
    /// </summary>
    public IManialinkAction Action { get; }
    
    /// <summary>
    /// The FormEntry model which is assigned to the current action.
    /// </summary>
    public object? EntryModel { get; }
}
