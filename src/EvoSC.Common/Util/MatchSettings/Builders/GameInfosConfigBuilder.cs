using System.Xml;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Common.Util.MatchSettings.Models;

namespace EvoSC.Common.Util.MatchSettings.Builders;

public class GameInfosConfigBuilder
{
    private int _gameMode = 0;
    private int _chatTime = 10000;
    private int _finishTimeout = 1;
    private bool _allowWarmupDuration = false;
    private bool _disableRespawn = false;
    private bool _forceShowAllOpponents = false;
    private string _scriptName = "";

    public string ScriptName => _scriptName;

    public GameInfosConfigBuilder WithGameMode(int gameMode)
    {
        _gameMode = gameMode;
        return this;
    }

    public GameInfosConfigBuilder WithChatTime(int chatTime)
    {
        _chatTime = chatTime;
        return this;
    }

    public GameInfosConfigBuilder WithFinishTimeout(int timeout)
    {
        _finishTimeout = timeout;
        return this;
    }

    public GameInfosConfigBuilder AllowWarmupDuration(bool allow)
    {
        _allowWarmupDuration = allow;
        return this;
    }

    public GameInfosConfigBuilder DisableRespawn(bool disable)
    {
        _disableRespawn = disable;
        return this;
    }

    public GameInfosConfigBuilder ForceShowAllOpponents(bool force)
    {
        _forceShowAllOpponents = force;
        return this;
    }

    public GameInfosConfigBuilder WithScriptName(DefaultModeScriptName scriptName)
    {
        _scriptName = scriptName.GetIdentifier();
        return this;
    }
    
    public GameInfosConfigBuilder WithScriptName(string scriptName)
    {
        _scriptName = scriptName;
        return this;
    }

    public MatchSettingsGameInfos Build()
    {
        if (_scriptName == null)
        {
            throw new InvalidOperationException("GameInfos cannot be built without a script name.");
        }
        
        return new MatchSettingsGameInfos
        {
            GameMode = _gameMode,
            ChatTime = _chatTime,
            FinishTimeout = _finishTimeout,
            AllowWarmupDuration = _allowWarmupDuration,
            DisableRespawn = _disableRespawn,
            ForceShowAllOpponents = _forceShowAllOpponents,
            ScriptName = _scriptName
        };
    }
}
