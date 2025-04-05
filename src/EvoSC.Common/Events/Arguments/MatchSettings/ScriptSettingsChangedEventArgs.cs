namespace EvoSC.Common.Events.Arguments.MatchSettings;

public class ScriptSettingsChangedEventArgs : EventArgs
{
    public Dictionary<string, object> NewScriptSettings { get; init; }
}
