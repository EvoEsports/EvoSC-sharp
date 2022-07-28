using System;
using System.Collections.Generic;
using EvoSC.Core.Commands.Generic;
using EvoSC.Core.Commands.Generic.Interfaces;

namespace EvoSC.Core.Commands.Chat;

public class ChatCommandParserResult : ICommandParserResult
{
    public bool IsSuccess { get; }
    public Exception? Exception { get; }
    public ICommand? Command { get; }
    public IEnumerable<object> Arguments { get; }

    public ChatCommandParserResult(bool success, ICommand? command, IEnumerable<object> args, Exception? exception=null)
    {
        IsSuccess = success;
        Command = command;
        Arguments = args;
        Exception = exception;
    }
}
