using EvoSC.Common.Exceptions;

namespace EvoSC.CLI.Exceptions;

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
