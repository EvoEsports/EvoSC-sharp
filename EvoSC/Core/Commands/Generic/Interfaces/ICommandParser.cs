using System.Threading.Tasks;

namespace EvoSC.Core.Commands.Generic.Interfaces;

public interface ICommandParser
{
    public Task<ICommandParserResult> Parse(string input);
}
