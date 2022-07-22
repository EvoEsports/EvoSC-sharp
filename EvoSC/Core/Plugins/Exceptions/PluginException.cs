using System;
using EvoSC.Core.Exceptions;

namespace EvoSC.Core.Plugins.Exceptions;

public class PluginException : EvoSCException
{
    public PluginException(){}
    public PluginException(string message) : base(message){}
    public PluginException(string message, Exception innerException) : base(message, innerException){}
}
