namespace EvoSC.Manialinks.Interfaces.Models;

/// <summary>
/// Holds information about the execution context of a Manialink Action.
/// </summary>
public interface IManialinkActionContext
{
    /// <summary>
    /// Information about the current Manialink Action.
    /// </summary>
    public IManialinkAction Action { get; }
    
    /// <summary>
    /// The FormEntry model which is assigned to the current action.
    /// </summary>
    public object? EntryModel { get; }
}
