using EvoSC.Common.Exceptions;

namespace EvoSC.Common.Interfaces;

public class InvalidControllerClassException : ControllerException
{
    public InvalidControllerClassException(string message) : base(message)
    {
    }
}
