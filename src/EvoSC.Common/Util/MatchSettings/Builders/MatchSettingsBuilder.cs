using System.ComponentModel;
using System.Reflection;
using System.Xml;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Common.Util.MatchSettings.Attributes;
using EvoSC.Common.Util.MatchSettings.Models;

namespace EvoSC.Common.Util.MatchSettings.Builders;

public class MatchSettingsBuilder
{
    private GameInfosConfigBuilder _gameInfosbuilder = new();
    private FilterConfigBuilder _filterBuilder = new();
    private Dictionary<string, ModeScriptSetting> _scriptSettings;
    private List<IMap> _maps = new();
    private int _startIndex;

    public MatchSettingsBuilder()
    {
    }
    
    public MatchSettingsBuilder(IMatchSettings matchSettings)
    {
        _gameInfosbuilder = new GameInfosConfigBuilder(matchSettings.GameInfos);
        _filterBuilder = new FilterConfigBuilder(matchSettings.Filter);
        _scriptSettings = matchSettings.ModeScriptSettings ?? new Dictionary<string, ModeScriptSetting>();
        _maps = matchSettings.Maps ?? new List<IMap>();
        _startIndex = matchSettings.StartIndex;
    }

    public MatchSettingsBuilder WithMode(DefaultModeScriptName mode)
    {
        _gameInfosbuilder.WithScriptName(mode);
        return this;
    }
    
    public MatchSettingsBuilder WithMode(string scriptName)
    {
        _gameInfosbuilder.WithScriptName(scriptName);
        return this;
    }
    
    public MatchSettingsBuilder WithGameInfos(Action<GameInfosConfigBuilder> builderAction)
    {
        builderAction(_gameInfosbuilder);
        return this;
    }

    public MatchSettingsBuilder WithGameInfos(GameInfosConfigBuilder newGameInfosBuilder)
    {
        _gameInfosbuilder = newGameInfosBuilder;
        return this;
    }

    public MatchSettingsBuilder WithFilter(Action<FilterConfigBuilder> builderAction)
    {
        builderAction(_filterBuilder);
        return this;
    }

    public MatchSettingsBuilder WithGameInfos(FilterConfigBuilder newFilterBuilder)
    {
        _filterBuilder = newFilterBuilder;
        return this;
    }

    public MatchSettingsBuilder WithStartIndex(int index)
    {
        _startIndex = index;
        return this;
    }

    public MatchSettingsBuilder AddMap(IMap map)
    {
        _maps.Add(map);
        return this;
    }

    public MatchSettingsBuilder AddMap(string fileName)
    {
        _maps.Add(new Map {FilePath = fileName});
        return this;
    }
    
    public MatchSettingsBuilder AddMap(string fileName, string uid)
    {
        _maps.Add(new Map {FilePath = fileName, Uid = uid});
        return this;
    }

    /// <summary>
    /// Set the settings for this match settings. Make sure to
    /// call WithMode() first!
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

    public MatchSettingsBuilder WithModeSettings(Action<Dictionary<string, object?>> settingsAction)
    {
        if (_scriptSettings == null)
        {
            _scriptSettings = new Dictionary<string, ModeScriptSetting>();
        }

        var settingsToSet = new Dictionary<string, object?>();
        settingsAction(settingsToSet);

        foreach (var (name, value) in settingsToSet)
        {
            _scriptSettings[name] = new ModeScriptSetting {Value = value, Description = "", Type = value.GetType()};
        }

        return this;
    }

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

    private Dictionary<string, ModeScriptSetting> GetScriptSettings(ModeScriptSettings settingsObject)
    {
        var settings = new Dictionary<string, ModeScriptSetting>();

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
            var defaultValueAttrs = effectiveProperty.GetCustomAttributes<DefaultScriptSettingValue>();

            var propertyValue = property.GetValue(settingsObject);

            var setting = new ModeScriptSetting
            {
                Value = propertyValue ?? GetDefaultValue(defaultValueAttrs),
                Description = descAttr?.Description ?? "",
                Type = property.PropertyType
            };
            
            settings.Add(settingAttr.Name, setting);
        }

        return settings;
    }

    private object? GetDefaultValue(IEnumerable<DefaultScriptSettingValue> defaultValues)
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
