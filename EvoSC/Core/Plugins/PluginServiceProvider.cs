using System;
using System.Collections.Generic;
using EvoSC.Core.Plugins.Abstractions;

namespace EvoSC.Core.Plugins;

public class PluginServiceProvider : IPluginServiceProvider
{
    private readonly IServiceProvider _mainServices;
    private readonly IDictionary<Guid, IServiceProvider> _pluginServices;

    public PluginServiceProvider(IServiceProvider mainServices)
    {
        _mainServices = mainServices;
        _pluginServices = new Dictionary<Guid, IServiceProvider>();
    }
    
    public object? GetService(Type serviceType)
    {
        var service = _mainServices.GetService(serviceType);

        if (service == null)
        {
            // try to find the service in the plugin services instead
            foreach (var provider in _pluginServices.Values)
            {
                service = provider.GetService(serviceType);

                if (service != null)
                {
                    return service;
                }
            }
        }

        return service;
    }

    public void AddProvider(Guid pluginId, IServiceProvider provider)
    {
        _pluginServices.TryAdd(pluginId, provider);
    }

    public void RemoveProvider(Guid pluginId)
    {
        if (!_pluginServices.ContainsKey(pluginId))
        {
            throw new KeyNotFoundException($"The plugin with id '{pluginId}' doesn't exist.");
        }

        _pluginServices.Remove(pluginId);
    }
}
