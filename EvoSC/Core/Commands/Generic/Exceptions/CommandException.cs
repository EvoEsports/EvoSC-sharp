using System;

namespace EvoSC.Core.Commands.Generic.Exceptions;

public class CommandException : Exception
{
    public CommandException(string message) : base(message) {}
}
