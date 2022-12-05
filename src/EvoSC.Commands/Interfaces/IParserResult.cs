namespace EvoSC.Commands.Interfaces;

public interface IParserResult
{
    /// <summary>
    /// The command that was parsed and found.
    /// </summary>
    public IChatCommand Command { get; }
    /// <summary>
    /// Arguments provided for the command handler.
    /// </summary>
    public IEnumerable<object> Arguments { get; }
    /// <summary>
    /// Whether parsing of the input was successful.
    /// </summary>
    public bool Success { get; }
    /// <summary>
    /// Exception containing information of any errors that occured during parsing. If Success is false,
    /// this will be set. But if Success is true, this may or may not be set.
    /// </summary>
    public Exception Exception { get; }
    public bool IsIntended { get; }
    public string AliasUsed { get; }
}
