using EvoSC.Commands.Interfaces;

namespace EvoSC.Commands.Parser;

public class ParserResult : IParserResult
{
    public IChatCommand Command { get; init; }
    public IEnumerable<object> Arguments { get; init; }
    public bool Success { get; init; }
    public Exception Exception { get; }

    public ParserResult(IChatCommand? cmd, IEnumerable<object> args, bool success, Exception ex=null)
    {
        Command = cmd;
        Arguments = args;
        Success = success;
        Exception = ex;
    }

    public ParserResult(IChatCommand? cmd, IEnumerable<object> args, bool success) : this(cmd, args, success, null)
    {
    }
}
