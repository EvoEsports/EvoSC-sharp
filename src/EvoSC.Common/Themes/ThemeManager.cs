using System.Reflection;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Themes.Events;
using EvoSC.Common.Themes.Events.Args;
using EvoSC.Common.Themes.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Common.Themes;

public class ThemeManager : IThemeManager
{
    private readonly IServiceContainerManager _serviceManager;
    private readonly IEvoSCApplication _evoscApp;
    private readonly IEventManager _events;
    private readonly IEvoScBaseConfig _evoscConfig;

    private dynamic? _themeOptionsCache = null;
    
    private Dictionary<string, IThemeInfo> _availableThemes = new();
    
    public ITheme? SelectedTheme { get; private set; }
    public IEnumerable<IThemeInfo> AvailableThemes => _availableThemes.Values;
    public dynamic Theme => _themeOptionsCache ?? GetCurrentThemeOptions();


    public ThemeManager(IServiceContainerManager serviceManager, IEvoSCApplication evoscApp, IEventManager events, IEvoScBaseConfig evoscConfig)
    {
        _serviceManager = serviceManager;
        _evoscApp = evoscApp;
        _events = events;
        _evoscConfig = evoscConfig;

        AddThemeAsync(typeof(DefaultTheme));
    }

    public async Task AddThemeAsync(Type themeType, Guid moduleId)
    {
        var attr = themeType.GetCustomAttribute<ThemeAttribute>();

        if (attr == null)
        {
            throw new InvalidOperationException($"The provided theme type {themeType} does not annotate the Theme attribute.");
        }

        if (_availableThemes.ContainsKey(attr.Name))
        {
            throw new ThemeException($"A theme with the name '{attr.Name}' already exists.");
        }

        _availableThemes[attr.Name] = new ThemeInfo
        {
            ThemeType = themeType,
            Name = attr.Name,
            Description = attr.Description,
            ModuleId = moduleId
        };

        if (_availableThemes.Count == 1)
        {
            await ActivateThemeAsync(attr.Name);
        }
    }
    
    public Task AddThemeAsync(Type themeType) => AddThemeAsync(themeType, Guid.Empty);

    public void RemoveTheme(string name)
    {
        ThrowIfNotExists(name);

        var themeInfo = _availableThemes[name];

        if (SelectedTheme != null && SelectedTheme.GetType() == themeInfo.ThemeType)
        {
            throw new ThemeException("Cannot remove current theme. Change to another theme first.");
        }
        
        _availableThemes.Remove(name);
    }

    public void RemoveThemesForModule(Guid moduleId)
    {
        foreach (var (name, theme) in _availableThemes)
        {
            if (theme.ModuleId.Equals(moduleId))
            {
                _availableThemes.Remove(name);
            }
        }
    }

    public Task SetCurrentThemeAsync(string name)
    {
        ThrowIfNotExists(name);
        return ActivateThemeAsync(name);
    }

    public dynamic GetCurrentThemeOptions()
    {
        if (SelectedTheme == null)
        {
            throw new ThemeException("Current theme is not set.");
        }

        var fallbackOptions = GetFallbackThemeOptions();
        var themeOptions = new DynamicThemeOptions(fallbackOptions);

        foreach (var option in SelectedTheme.ThemeOptions)
        {
            themeOptions[option.Key] = option.Value;
        }

        _themeOptionsCache = themeOptions;
        return themeOptions;
    }

    private async Task<ITheme> ActivateThemeAsync(string name)
    {
        var themeInfo = _availableThemes[name];
        var services = _evoscApp.Services;

        if (!themeInfo.ModuleId.Equals(Guid.Empty))
        {
            services = _serviceManager.GetContainer(themeInfo.ModuleId);
        }

        var theme = ActivatorUtilities.CreateInstance(services, themeInfo.ThemeType) as ITheme;

        if (theme == null)
        {
            throw new ThemeException($"Failed to activate theme '{name}'.");
        }
        
        await theme.ConfigureAsync();
        SelectedTheme = theme;
        
        // invalidate options cache
        _themeOptionsCache = null;

        await _events.RaiseAsync(ThemeEvents.CurrentThemeChanged, new ThemeChangedEventArgs
        {
            ThemeInfo = themeInfo,
            Theme = theme
        });
        
        return theme;
    }

    private void ThrowIfNotExists(string name)
    {
        if (!_availableThemes.ContainsKey(name))
        {
            throw new ThemeDoesNotExistException(name);
        }
    }

    private Dictionary<string, object> GetFallbackThemeOptions()
    {
        var themeOptions = new Dictionary<string, object>();

        foreach (var defaultOption in _evoscConfig.Theme)
        {
            var key = defaultOption.Key.StartsWith("Theme.") ? defaultOption.Key[6..] : defaultOption.Key;
            
            themeOptions[key] = defaultOption.Value;
        }

        return themeOptions;
    }
}
