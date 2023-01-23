namespace EvoSC.Common.Exceptions;

/// <summary>
/// Thrown when the controller's class is invalid. Eg. not extending or implementing the right interfaces/classes.
/// </summary>
public class InvalidControllerClassException : ControllerException
{
    public InvalidControllerClassException(string message) : base(message)
    {
    }
}
