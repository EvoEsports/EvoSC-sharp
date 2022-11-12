namespace EvoSC.Commands.Exceptions;

/// <summary>
/// Thrown when the input does not have enough arguments to call a command's handler.
/// </summary>
public class NotEnoughArgumentsException : CommandParserException
{
    private readonly int _requiredArgs;
    
    public NotEnoughArgumentsException(int requiredArgs, bool intendedCommand) : base(intendedCommand)
    {
        _requiredArgs = requiredArgs;
    }

    public override string Message => $"The command requires {_requiredArgs} argument(s).";
}
