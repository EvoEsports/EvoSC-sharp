using System.Collections.Generic;

namespace EvoSC.Core.Commands.Generic.Interfaces;

public class ICommandParserResult
{
    public Command Command { get; }
    public IEnumerable<object> Arguments { get; }
}
