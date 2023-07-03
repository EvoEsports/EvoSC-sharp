namespace EvoSC.CLI.Exceptions;

public class EvoScCliException : Exception
{
    public EvoScCliException()
    {
    }

    public EvoScCliException(string? message) : base(message)
    {
    }

    public EvoScCliException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
