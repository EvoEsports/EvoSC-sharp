namespace EvoSC.Common.Exceptions.DatabaseExceptions;

public class QueryCompilationFailedException : EvoSCException
{
    public QueryCompilationFailedException() : base("The query failed to compile. Is the syntax correct?")
    {
    }
}
