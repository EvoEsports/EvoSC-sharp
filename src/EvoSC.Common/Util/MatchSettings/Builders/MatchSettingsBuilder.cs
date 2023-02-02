using System.Reflection;
using System.Xml;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Util.MatchSettings.Builders;

public class MatchSettingsBuilder
{
    private GameInfosConfigBuilder _gameInfosbuilder = new();
    private FilterConfigBuilder _filterBuilder = new();
    private ModeScriptSettings _scriptSettings;
    private int _startIndex;

    public MatchSettingsBuilder()
    {
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
        _scriptSettings = settings;
        return this;
    }

    public Models.MatchSettingsInfo Build()
    {
        return new Models.MatchSettingsInfo
        {
            GameInfos = _gameInfosbuilder.Build(),
            Filter = _filterBuilder.Build(),
            ModeScriptSettings = _scriptSettings,
            StartIndex = _startIndex
        };
    }
}
