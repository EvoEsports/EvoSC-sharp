namespace EvoSC.Commands.Exceptions;

/// <summary>
/// Thrown when a command or alias is registered more than one time.
/// </summary>
public class DuplicateChatCommandException : CommandException
{
    private readonly string _name;
    
    public DuplicateChatCommandException(string name)
    {
        _name = name;
    }

    public override string Message => $"Chat command with name '{_name}' already exists.";
}
