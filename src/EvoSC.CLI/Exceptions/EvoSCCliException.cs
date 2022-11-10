using EvoSC.Common.Exceptions;

namespace EvoSC.CLI.Exceptions;

/// <summary>
/// General exception for the CLI handler.
/// </summary>
public class EvoSCCliException : EvoSCException
{
    public EvoSCCliException(string? message) : base(message)
    {
    }

    public EvoSCCliException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public EvoSCCliException()
    {
    }
}
