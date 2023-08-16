using GbxRemoteNet.Structs;

namespace EvoSC.Common.Remote.EventArgsModels;

public class MapEventArgs : EventArgs
{
    public int Count;
    public int Time;
    public TmSMapInfo Map;
}
