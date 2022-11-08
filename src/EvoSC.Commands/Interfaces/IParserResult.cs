namespace EvoSC.Commands.Interfaces;

public interface IParserResult
{
    public IChatCommand Command { get; }
    public IEnumerable<object> Arguments { get; }
    public bool Success { get; }
    public Exception Exception { get; }
}
