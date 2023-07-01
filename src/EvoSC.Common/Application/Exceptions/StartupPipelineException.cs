namespace EvoSC.Common.Application.Exceptions;

public class StartupPipelineException : Exception
{
    public StartupPipelineException()
    {
    }

    public StartupPipelineException(string? message) : base(message)
    {
    }

    public StartupPipelineException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
