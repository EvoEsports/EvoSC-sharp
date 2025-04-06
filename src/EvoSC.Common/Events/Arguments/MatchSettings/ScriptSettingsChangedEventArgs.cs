namespace EvoSC.Common.Events.Arguments.MatchSettings;

public class ScriptSettingsChangedEventArgs : EventArgs
{
    /// <summary>
    /// The new script settings that were set.
    /// </summary>
    public Dictionary<string, object> NewScriptSettings { get; init; }
}
