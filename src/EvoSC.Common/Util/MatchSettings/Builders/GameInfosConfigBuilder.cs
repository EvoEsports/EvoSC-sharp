using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Common.Util.MatchSettings.Models;

namespace EvoSC.Common.Util.MatchSettings.Builders;

/// <summary>
/// Fluent builder for building the gameinfos section in a MatchSettings file.
/// </summary>
public class GameInfosConfigBuilder
{
    private int _gameMode;
    private int _chatTime = 10000;
    private int _finishTimeout = 1;
    private bool _allWarmupDuration;
    private bool _disableRespawn;
    private bool _forceShowAllOpponents;
    private string _scriptName = "";

    public GameInfosConfigBuilder(){}
    
    /// <summary>
    /// Create the builder with an existing gameinfos object.
    /// </summary>
    /// <param name="gameInfos">The gameinfos object to begin with.</param>
    public GameInfosConfigBuilder(MatchSettingsGameInfos gameInfos)
    {
        _gameMode = gameInfos.GameMode;
        _chatTime = gameInfos.ChatTime;
        _finishTimeout = gameInfos.ChatTime;
        _allWarmupDuration = gameInfos.AllWarmupDuration;
        _disableRespawn = gameInfos.DisableRespawn;
        _forceShowAllOpponents = gameInfos.ForceShowAllOpponents;
        _scriptName = gameInfos.ScriptName;
    }

    /// <summary>
    /// The current game mode name.
    /// </summary>
    public string ScriptName => _scriptName;

    /// <summary>
    /// Set the game mode number. This is typically not used for anything.
    /// Use WithScriptName to set the actual game mode script.
    /// </summary>
    /// <param name="gameMode"></param>
    /// <returns></returns>
    public GameInfosConfigBuilder WithGameMode(int gameMode)
    {
        _gameMode = gameMode;
        return this;
    }

    /// <summary>
    /// Set the chat time.
    /// </summary>
    /// <param name="chatTime">Chat time in milliseconds.</param>
    /// <returns></returns>
    public GameInfosConfigBuilder WithChatTime(int chatTime)
    {
        _chatTime = chatTime;
        return this;
    }

    /// <summary>
    /// Set the finish timeout.
    /// </summary>
    /// <param name="timeout">Timeout in seconds.</param>
    /// <returns></returns>
    [Obsolete("Not used in TM20202 anymore.")]
    public GameInfosConfigBuilder WithFinishTimeout(int timeout)
    {
        _finishTimeout = timeout;
        return this;
    }

    /// <summary>
    /// Set the all warmup duration.
    /// </summary>
    /// <param name="enable">Enable or disable this option.</param>
    /// <returns></returns>
    public GameInfosConfigBuilder AllWarmupDuration(bool enable)
    {
        _allWarmupDuration = enable;
        return this;
    }

    /// <summary>
    /// Enable or disable respawn.
    /// </summary>
    /// <param name="disable"></param>
    /// <returns></returns>
    [Obsolete("Not used in TM2020 anymore")]
    public GameInfosConfigBuilder DisableRespawn(bool disable)
    {
        _disableRespawn = disable;
        return this;
    }

    /// <summary>
    /// Set forcing showing of all opponents.
    /// </summary>
    /// <param name="force">Enable or disable.</param>
    /// <returns></returns>
    public GameInfosConfigBuilder ForceShowAllOpponents(bool force)
    {
        _forceShowAllOpponents = force;
        return this;
    }
    
    /// <summary>
    /// Set the game mode for this match settings.
    /// </summary>
    /// <param name="scriptName">The name of the game mode script.</param>
    /// <returns></returns>
    public GameInfosConfigBuilder WithScriptName(DefaultModeScriptName scriptName)
    {
        _scriptName = scriptName.GetIdentifier();
        return this;
    }
    
    /// <summary>
    /// Set the game mode for this match settings.
    /// </summary>
    /// <param name="scriptName">The name of the game mode script.</param>
    /// <returns></returns>
    public GameInfosConfigBuilder WithScriptName(string scriptName)
    {
        _scriptName = scriptName;
        return this;
    }

    /// <summary>
    /// Create the gameinfos object with the current values.
    /// Note: Keep in mind that the ScriptName must be set before building!
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Thrown if the script name is not set.</exception>
    public MatchSettingsGameInfos Build()
    {
        if (string.IsNullOrEmpty(_scriptName))
        {
            throw new InvalidOperationException("GameInfos cannot be built without a script name.");
        }
        
        return new MatchSettingsGameInfos
        {
            GameMode = _gameMode,
            ChatTime = _chatTime,
            FinishTimeout = _finishTimeout,
            AllWarmupDuration = _allWarmupDuration,
            DisableRespawn = _disableRespawn,
            ForceShowAllOpponents = _forceShowAllOpponents,
            ScriptName = _scriptName
        };
    }
}
