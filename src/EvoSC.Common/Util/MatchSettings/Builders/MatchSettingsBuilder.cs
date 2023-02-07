using System.ComponentModel;
using System.Reflection;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Util.MatchSettings.Attributes;
using EvoSC.Common.Util.MatchSettings.Models;

namespace EvoSC.Common.Util.MatchSettings.Builders;

/// <summary>
/// Fluent builder for creating or editing MatchSettings.
/// </summary>
public class MatchSettingsBuilder
{
    private GameInfosConfigBuilder _gameInfosbuilder = new();
    private FilterConfigBuilder _filterBuilder = new();
    private Dictionary<string, ModeScriptSettingInfo> _scriptSettings;
    private List<IMap> _maps = new();
    private int _startIndex;

    public MatchSettingsBuilder()
    {
    }
    
    /// <summary>
    /// Create a builder from an existing MatchSettings, and provide editing
    /// functionality.
    /// </summary>
    /// <param name="matchSettings">The match settings to edit.</param>
    public MatchSettingsBuilder(IMatchSettings matchSettings)
    {
        _gameInfosbuilder = new GameInfosConfigBuilder(matchSettings.GameInfos);
        _filterBuilder = new FilterConfigBuilder(matchSettings.Filter);
        _scriptSettings = matchSettings.ModeScriptSettings ?? new Dictionary<string, ModeScriptSettingInfo>();
        _maps = matchSettings.Maps ?? new List<IMap>();
        _startIndex = matchSettings.StartIndex;
    }

    /// <summary>
    /// Set the game mode for this MatchSettings using a default mode.
    /// </summary>
    /// <param name="mode">The game mode.</param>
    /// <returns></returns>
    public MatchSettingsBuilder WithMode(DefaultModeScriptName mode)
    {
        _gameInfosbuilder.WithScriptName(mode);
        return this;
    }
    
    /// <summary>
    /// Set the game mode for this MatchSettings using a custom mode.
    /// </summary>
    /// <param name="scriptName"></param>
    /// <returns></returns>
    public MatchSettingsBuilder WithMode(string scriptName)
    {
        _gameInfosbuilder.WithScriptName(scriptName);
        return this;
    }
    
    /// <summary>
    /// Edit the gameinfos for this MatchSettings.
    /// </summary>
    /// <param name="builderAction">Fluent builder for setting gameinfos.</param>
    /// <returns></returns>
    public MatchSettingsBuilder WithGameInfos(Action<GameInfosConfigBuilder> builderAction)
    {
        builderAction(_gameInfosbuilder);
        return this;
    }

    /// <summary>
    /// Set the gameinfos for this MatchSettings.
    /// </summary>
    /// <param name="newGameInfosBuilder">A builder for this gameinfos.</param>
    /// <returns></returns>
    public MatchSettingsBuilder WithGameInfos(GameInfosConfigBuilder newGameInfosBuilder)
    {
        _gameInfosbuilder = newGameInfosBuilder;
        return this;
    }

    /// <summary>
    /// Edit the filter options for this MatchSettings.
    /// </summary>
    /// <param name="builderAction">Fluent builder for this filter.</param>
    /// <returns></returns>
    public MatchSettingsBuilder WithFilter(Action<FilterConfigBuilder> builderAction)
    {
        builderAction(_filterBuilder);
        return this;
    }

    /// <summary>
    /// Set the filter options for this MatchSettings.
    /// </summary>
    /// <param name="newFilterBuilder">A builder for this filter.</param>
    /// <returns></returns>
    public MatchSettingsBuilder WithFilter(FilterConfigBuilder newFilterBuilder)
    {
        _filterBuilder = newFilterBuilder;
        return this;
    }

    /// <summary>
    /// Set the map start index for this MatchSettings.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public MatchSettingsBuilder WithStartIndex(int index)
    {
        _startIndex = index < 0 ? 0 : index;
        return this;
    }

    /// <summary>
    /// Add a map to this MatchSettings.
    /// </summary>
    /// <param name="map">The map to add.</param>
    /// <returns></returns>
    public MatchSettingsBuilder AddMap(IMap map)
    {
        _maps.Add(map);
        return this;
    }

    /// <summary>
    /// Add a map by it's filename to the MatchSettings.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public MatchSettingsBuilder AddMap(string fileName)
    {
        _maps.Add(new Map {FilePath = fileName});
        return this;
    }
    
    /// <summary>
    /// Add a map to the map list.
    /// </summary>
    /// <param name="fileName">The filename of the map.</param>
    /// <param name="uid">The unique ID of the map.</param>
    /// <returns></returns>
    public MatchSettingsBuilder AddMap(string fileName, string uid)
    {
        _maps.Add(new Map {FilePath = fileName, Uid = uid});
        return this;
    }

    /// <summary>
    /// Add a list of maps to the map list.
    /// </summary>
    /// <param name="maps">The maps to add.</param>
    /// <returns></returns>
    public MatchSettingsBuilder AddMaps(IEnumerable<IMap> maps)
    {
        _maps.AddRange(maps);
        return this;
    }

    /// <summary>
    /// Set the maps for this match settings. This overwrites
    /// any previous maps set.
    /// </summary>
    /// <param name="maps">The maps to set.</param>
    /// <returns></returns>
    public MatchSettingsBuilder WithMaps(IEnumerable<IMap> maps)
    {
        _maps = maps.ToList();
        return this;
    }

    /// <summary>
    /// Set the settings for this match settings. Requires WithMode() without
    /// an empty script name to be called first.
    /// </summary>
    /// <param name="settingsAction"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public MatchSettingsBuilder WithModeSettings<T>(Action<T> settingsAction)
        where T : ModeScriptSettings, new()
    {
        if (_gameInfosbuilder.ScriptName.Trim() != "")
        {
            var t = typeof(T);
            var scriptNameAttr = t.GetCustomAttributes<ScriptSettingsForAttribute>().FirstOrDefault();

            if (scriptNameAttr == null || !scriptNameAttr.Name.Equals(_gameInfosbuilder.ScriptName, StringComparison.Ordinal))
            {
                throw new InvalidOperationException("The provided settings type does not match the game mode set.");
            }
        }

        var settings = new T();
        settingsAction(settings);
        _scriptSettings = GetScriptSettings(settings);
        
        return this;
    }

    /// <summary>
    /// Set the settings for this MatchSettings. Requires WithMode() without
    /// an empty script name to be called first.
    /// </summary>
    /// <param name="settingsAction">Fluent action for setting the settings dictionary.</param>
    /// <returns></returns>
    public MatchSettingsBuilder WithModeSettings(Action<Dictionary<string, object?>> settingsAction)
    {
        if (_scriptSettings == null)
        {
            _scriptSettings = new Dictionary<string, ModeScriptSettingInfo>();
        }

        var settingsToSet = new Dictionary<string, object?>();
        settingsAction(settingsToSet);

        foreach (var (name, value) in settingsToSet)
        {
            _scriptSettings[name] = new ModeScriptSettingInfo {Value = value, Description = "", Type = value.GetType()};
        }

        return this;
    }

    /// <summary>
    /// Create the match settings object from the current values.
    /// </summary>
    /// <returns></returns>
    public MatchSettingsInfo Build()
    {
        return new MatchSettingsInfo
        {
            GameInfos = _gameInfosbuilder.Build(),
            Filter = _filterBuilder.Build(),
            ModeScriptSettings = _scriptSettings,
            Maps = _maps,
            StartIndex = _startIndex
        };
    }

    /// <summary>
    /// Obtain script settings from a script settings representation object.
    /// </summary>
    /// <param name="settingsObject">The object with available script settings for the mode.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private Dictionary<string, ModeScriptSettingInfo> GetScriptSettings(ModeScriptSettings settingsObject)
    {
        var settings = new Dictionary<string, ModeScriptSettingInfo>();

        var classType = settingsObject.GetType();

        var properties = classType.GetProperties(
            BindingFlags.Instance
            | BindingFlags.Public
            | BindingFlags.DeclaredOnly
        );

        foreach (var property in properties)
        {
            var effectiveProperty = property;

            if (classType.BaseType != null)
            {
                var propertyDefinition = MatchSettingsMapper.FindBasePropertyDefinition(classType.BaseType, property.Name);

                if (propertyDefinition != null)
                {
                    effectiveProperty = propertyDefinition;
                }
            }

            var settingAttr = effectiveProperty.GetCustomAttribute<ScriptSettingAttribute>() ??
                              throw new InvalidOperationException(
                                  $"The property '{property.Name}' must annotate ScriptSetting.");

            var descAttr = effectiveProperty.GetCustomAttribute<DescriptionAttribute>();
            var defaultValueAttrs = effectiveProperty.GetCustomAttributes<DefaultScriptSettingValueAttribute>();

            var propertyValue = property.GetValue(settingsObject);

            var setting = new ModeScriptSettingInfo
            {
                Value = propertyValue ?? GetDefaultValue(defaultValueAttrs),
                Description = descAttr?.Description ?? "",
                Type = property.PropertyType
            };
            
            settings.Add(settingAttr.Name, setting);
        }

        return settings;
    }

    /// <summary>
    /// Get the default value of a setting.
    /// </summary>
    /// <param name="defaultValues"></param>
    /// <returns></returns>
    private object? GetDefaultValue(IEnumerable<DefaultScriptSettingValueAttribute> defaultValues)
    {
        foreach (var defaultValue in defaultValues)
        {
            if (defaultValue.OnMode == null)
            {
                return defaultValue.Value;
            }
            
            if (defaultValue.OnMode.Equals(_gameInfosbuilder.ScriptName, StringComparison.Ordinal))
            {
                return defaultValue.Value;
            }
        }

        return null;
    }
}
