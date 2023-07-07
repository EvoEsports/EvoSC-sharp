namespace EvoSC.CLI.Exceptions;

/// <summary>
/// Exception that occured within the CLI handler.
/// </summary>
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
