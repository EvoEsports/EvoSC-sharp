namespace EvoSC.Commands.Exceptions;

public class CommandNotFoundException : CommandParserException
{
    private readonly string _cmdName;
    private readonly bool _intendedCommand;
    
    public CommandNotFoundException(string cmdName, bool intendedCommand) : base(intendedCommand)
    {
        _cmdName = cmdName;
    }

    public override string Message => $"The command '{_cmdName}' was not found.";
}
