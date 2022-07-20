using System;

namespace EvoSC.Core.Plugins.Exceptions;

public class PluginNotLoadedException : PluginException
{
    public PluginNotLoadedException(Guid loadId) : base($"The plugin with load id '{loadId}' does not exit.")
    {
    }
}
