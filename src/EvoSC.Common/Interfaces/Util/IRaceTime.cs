namespace EvoSC.Common.Interfaces.Util;

public interface IRaceTime
{
    public int TotalMilliseconds { get; }
    public int Milliseconds { get; set; }
    public int Seconds { get; set; }
    public int Minutes { get; set; }
    public int Hours { get; set; }
}
