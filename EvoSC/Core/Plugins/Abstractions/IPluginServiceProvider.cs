using System;

namespace EvoSC.Core.Plugins.Abstractions;

public interface IPluginServiceProvider : IServiceProvider
{
    /// <summary>
    /// Add a new provider for a loaded plugin.
    /// </summary>
    /// <param name="pluginId">The load ID of the plugin.</param>
    /// <param name="provider">Service provider associated with the plugin.</param>
    public void AddProvider(Guid pluginId, IServiceProvider provider);
    /// <summary>
    /// Remove the service provider from a plugin.
    /// </summary>
    /// <param name="pluginId">The load ID of the plugin.</param>
    public void RemoveProvider(Guid pluginId);
}
