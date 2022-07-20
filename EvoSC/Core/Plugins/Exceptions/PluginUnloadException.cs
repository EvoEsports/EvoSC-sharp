using System;

namespace EvoSC.Core.Plugins.Exceptions;

public class PluginUnloadException : PluginException
{
    public PluginUnloadException(){}
    public PluginUnloadException(string message) : base(message){}
    public PluginUnloadException(string message, Exception innerException) : base(message, innerException){}
}
