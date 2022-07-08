using System;

namespace EvoSC.Core.Commands.Generic.Interfaces;

public interface ICommandResult
{
    public bool IsSuccess { get; }
    public Exception ExceptionThrown { get; }
}
