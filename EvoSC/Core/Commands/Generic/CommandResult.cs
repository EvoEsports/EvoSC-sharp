using System;
using EvoSC.Core.Commands.Generic.Interfaces;

namespace EvoSC.Core.Commands.Generic;

public class CommandResult : ICommandResult
{
    public bool IsSuccess { get; }
    public Exception Exception { get; }

    public CommandResult(bool isSuccess, Exception? exception=null)
    {
        IsSuccess = isSuccess;
        Exception = exception;
    }
}
