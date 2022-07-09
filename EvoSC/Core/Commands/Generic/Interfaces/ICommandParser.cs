using System.Threading.Tasks;
using EvoSC.Interfaces.Commands;

namespace EvoSC.Core.Commands.Generic.Interfaces;

public interface ICommandParser
{
    public Task<ICommandParserResult> Parse(string input);
}
