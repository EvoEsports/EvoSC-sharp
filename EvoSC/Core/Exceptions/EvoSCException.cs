using System;

namespace EvoSC.Core.Exceptions;

public class EvoSCException : Exception
{
    public EvoSCException(){}
    public EvoSCException(string message) : base(message) { }
    public EvoSCException(string message, Exception innerException) : base(message, innerException) { }
}
