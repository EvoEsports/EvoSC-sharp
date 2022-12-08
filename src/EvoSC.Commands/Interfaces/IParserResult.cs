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
    
    /// <summary>
    /// Did the user deliberately mean to execute this command? This is usually true when the user
    /// began the message with the command prefix.
    /// </summary>
    public bool IsIntended { get; }
    
    /// <summary>
    /// The alias that was used to execute this command.
    /// </summary>
    public string AliasUsed { get; }
}
