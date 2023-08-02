using EvoSC.Common.Interfaces.Parsing;

namespace EvoSC.Manialinks.Interfaces;

/// <summary>
/// Handles user input from XMLRPC and executes appropriate Manialink Actions.
/// </summary>
public interface IManialinkInteractionHandler
{
    /// <summary>
    /// Access the value reader manager for manialink interactions.
    /// </summary>
    public IValueReaderManager ValueReader { get; }
}
