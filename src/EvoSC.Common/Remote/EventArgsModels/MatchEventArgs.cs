namespace EvoSC.Common.Remote.EventArgsModels;

public class MatchEventArgs : EventArgs
{
    /// <summary>
    /// Each time a match event is called, this number is incremented by one.
    /// </summary>
    public int Count;
    /// <summary>
    /// Server time when the callback was sent.
    /// </summary>
    public int Time;
}
