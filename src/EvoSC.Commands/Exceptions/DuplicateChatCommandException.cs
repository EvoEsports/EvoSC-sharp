namespace EvoSC.Commands.Exceptions;

/// <summary>
/// Thrown when a command or alias is registered more than one time.
/// </summary>
public class DuplicateChatCommandException(string name) : CommandException
{
    public override string Message => $"Chat command with name '{name}' already exists.";
}
