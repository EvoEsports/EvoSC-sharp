using System.Collections;
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

public class ThemeManager(IServiceContainerManager serviceManager, IEvoSCApplication evoscApp, IEventManager events,
        IEvoScBaseConfig evoscConfig)
    : IThemeManager
{
    private dynamic? _themeOptionsCache;
    private Dictionary<string, string>? _componentReplacementsCache;
    
    private readonly Dictionary<string, IThemeInfo> _availableThemes = new();
    private readonly Dictionary<Type, ITheme> _activeThemes = new();
    
    public IEnumerable<IThemeInfo> AvailableThemes => _availableThemes.Values;
    public dynamic Theme => _themeOptionsCache ?? GetCurrentThemeOptions();

    public Dictionary<string, string> ComponentReplacements =>
        _componentReplacementsCache ?? GetCurrentComponentReplacements();

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

        var themeInfo = new ThemeInfo
        {
            ThemeType = themeType,
            Name = attr.Name,
            Description = attr.Description,
            ModuleId = moduleId,
            OverrideTheme = attr.OverrideTheme
        };

        _availableThemes[attr.Name] = themeInfo;

        if (!_activeThemes.ContainsKey(themeInfo.EffectiveThemeType))
        {
            await ActivateThemeAsync(attr.Name);
        }
    }
    
    public Task AddThemeAsync(Type themeType) => AddThemeAsync(themeType, Guid.Empty);

    public async Task RemoveThemeAsync(string name)
    {
        ThrowIfNotExists(name);

        var themeInfo = _availableThemes[name];
        
        _availableThemes.Remove(name);

        if (_activeThemes.Remove(themeInfo.EffectiveThemeType))
        {
            InvalidateCache();
            await events.RaiseAsync(ThemeEvents.CurrentThemeChanged, new ThemeUpdatedEventArgs());
        }
    }

    public async Task RemoveThemesForModuleAsync(Guid moduleId)
    {
        foreach (var (name, theme) in _availableThemes)
        {
            if (theme.ModuleId.Equals(moduleId))
            {
                await RemoveThemeAsync(name);
            }
        }
    }

    public async Task<ITheme> ActivateThemeAsync(string name)
    {
        ThrowIfNotExists(name);
        
        var themeInfo = _availableThemes[name];
        var services = evoscApp.Services;

        if (!themeInfo.ModuleId.Equals(Guid.Empty))
        {
            services = serviceManager.GetContainer(themeInfo.ModuleId);
        }

        var theme = ActivatorUtilities.CreateInstance(services, themeInfo.ThemeType) as ITheme;

        if (theme == null)
        {
            throw new ThemeException($"Failed to activate theme '{name}'.");
        }

        _activeThemes[themeInfo.EffectiveThemeType] = theme;
        
        await theme.ConfigureAsync(Theme);
        InvalidateCache();
        
        await theme.ConfigureDynamicAsync(Theme);
        InvalidateCache();

        await events.RaiseAsync(ThemeEvents.CurrentThemeChanged, new ThemeUpdatedEventArgs());

        return theme;
    }
    
    public void InvalidateCache()
    {
        _themeOptionsCache = null;
        _componentReplacementsCache = null;
    }
    
    private dynamic GetCurrentThemeOptions()
    {
        var configOverride = GetConfigOverrideOptions();
        var themeOptions = new DynamicThemeOptions(configOverride);

        foreach (var option in _activeThemes.Values.SelectMany(theme => theme.ThemeOptions))
        {
            themeOptions[option.Key] = option.Value;
        }
        
        // override options with whatever user has defined in the config
        foreach (var option in configOverride)
        {
            themeOptions[option.Key] = option.Value;
        }
        
        // override any options set as an envi
        foreach (DictionaryEntry enviObject in Environment.GetEnvironmentVariables())
        {
            var key = enviObject.Key as string;
            if (key == null || enviObject.Value == null || !key.StartsWith("EVOSC_THEME_", StringComparison.Ordinal))
            {
                continue;
            }

            foreach (var option in themeOptions)
            {
                var enviKey = $"EVOSC_THEME_{option.Key.Replace('.', '_').ToUpper()}";

                if (enviKey == key)
                {
                    themeOptions[option.Key] = enviObject.Value;
                }
            }
        }
        
        _themeOptionsCache = themeOptions;
        return themeOptions;
    }

    private void ThrowIfNotExists(string name)
    {
        if (!_availableThemes.ContainsKey(name))
        {
            throw new ThemeDoesNotExistException(name);
        }
    }

    private Dictionary<string, object> GetConfigOverrideOptions()
    {
        var themeOptions = new Dictionary<string, object>();

        foreach (var defaultOption in evoscConfig.Theme)
        {
            var key = defaultOption.Key.StartsWith("Theme.", StringComparison.Ordinal)
                ? defaultOption.Key[6..]
                : defaultOption.Key;

            themeOptions[key] = defaultOption.Value;
        }

        return themeOptions;
    }
    
    private Dictionary<string, string> GetCurrentComponentReplacements()
    {
        var replacements = new Dictionary<string, string>();
        
        foreach (var (component, replacement) in _activeThemes.Values.SelectMany(t => t.ComponentReplacements))
        {
            replacements[component] = replacement;
        }

        return replacements;
    }
}
