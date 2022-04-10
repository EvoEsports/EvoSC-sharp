using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace EvoSC.Core.Events;

public class EventManager : IDisposable
{
    private ILogger _logger;
    public EventManager(ILogger<EventManager> logger)
    {
        _logger = logger;
    }

    private Dictionary<EventType, Dictionary<Guid, Action>> _eventCache = new Dictionary<EventType, Dictionary<Guid, Action>>();
    private bool _disposedValue;

    public void RegisterEventType(EventType eventType, Guid pluginId, Action action)
    {
        if (!_eventCache.ContainsKey(eventType))
            _eventCache.Add(eventType, new Dictionary<Guid, Action>());

        if (!_eventCache[eventType].ContainsKey(pluginId))
            _eventCache[eventType].Add(pluginId, action);

        _logger.LogTrace($"Added new event of type '{eventType} for plugin {pluginId}");
    }

    public void UnregisterEvent(EventType eventType, Guid pluginId)
    {
        if (_eventCache.ContainsKey(eventType))
            _eventCache[eventType].Remove(pluginId);

        _logger.LogTrace($"Removed event of type '{eventType} for plugin {pluginId}");
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            _eventCache.Clear();
            _eventCache = null;

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}