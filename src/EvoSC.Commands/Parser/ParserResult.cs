using EvoSC.Commands.Interfaces;

namespace EvoSC.Commands.Parser;

public class ParserResult : IParserResult
{
    public required IChatCommand Command { get; init; }
    public required IEnumerable<object> Arguments { get; init; }
    public bool Success { get; init; }
    public Exception Exception { get; init; }
}
