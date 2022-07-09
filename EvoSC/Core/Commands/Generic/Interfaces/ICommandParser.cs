using System.Threading.Tasks;
using EvoSC.Interfaces.Commands;

namespace EvoSC.Core.Commands.Generic.Interfaces;

public interface ICommandParser<TResult> where TResult : ICommandParserResult
{
    public Task<TResult> Parse(string input);
}
