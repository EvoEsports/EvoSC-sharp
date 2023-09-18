using GbxRemoteNet.Structs;

namespace EvoSC.Common.Remote.EventArgsModels;

public class MapEventArgs : EventArgs
{
    /// <summary>
    /// Each time a map event is called, this number is incremented by one.
    /// </summary>
    public int Count;
    /// <summary>
    /// Server time when the callback was sent.
    /// </summary>
    public int Time;
    /// <summary>
    /// The trackmania map info object.
    /// </summary>
    public TmSMapInfo Map;
}
