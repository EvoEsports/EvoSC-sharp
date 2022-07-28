using System.Collections.Generic;

namespace EvoSC.Core.Commands.Generic.Interfaces;

public interface ICommandParserResult : IResult
{
    public ICommand? Command { get; }
    public IEnumerable<object> Arguments { get; }
}
