namespace EvoSC.Common.Application.Exceptions;

/// <summary>
/// General exception for the startup pipeline.
/// </summary>
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
