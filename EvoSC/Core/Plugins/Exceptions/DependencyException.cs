using System;

namespace EvoSC.Core.Plugins.Exceptions;

public class DependencyException : PluginException
{
    public DependencyException(){}
    public DependencyException(string message) : base(message){}
    public DependencyException(string message, Exception innerException) : base(message, innerException){}
}
