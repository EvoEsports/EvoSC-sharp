namespace EvoSC.Core.Plugins.Exceptions;

public class NoPluginClassException : PluginException
{
    public NoPluginClassException(string pluginName) : base($"Plugin class not found for '{pluginName}'")
    {
        
    }
}
