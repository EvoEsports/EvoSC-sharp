using System;

namespace EvoSC.Core.Commands.Generic.Interfaces;

public interface IResult
{
    public bool IsSuccess { get; }
    public Exception? Exception { get; }
}
