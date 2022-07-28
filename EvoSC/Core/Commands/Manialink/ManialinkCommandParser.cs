using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EvoSC.Core.Commands.Generic.Exceptions;
using EvoSC.Core.Commands.Generic.Interfaces;

namespace EvoSC.Core.Commands.Manialink;

public class ManialinkCommandParser : ICommandParser<ManialinkCommandParserResult>
{
    public Task<ManialinkCommandParserResult> Parse(string input)
    {
        throw new NotImplementedException();
    }
}
