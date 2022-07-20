namespace EvoSC.Core.Plugins.Exceptions;

public class PluginClassNotFoundException : PluginException
{
    public PluginClassNotFoundException(string pluginName) : base($"Plugin class not found for '{pluginName}'")
    {
        
    }
}
