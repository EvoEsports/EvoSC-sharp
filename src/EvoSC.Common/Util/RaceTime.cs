using System.Globalization;
using EvoSC.Common.Interfaces.Util;

namespace EvoSC.Common.Util;

public class RaceTime(int milliseconds, int seconds, int minutes, int hours)
    : IRaceTime
{
    public int TotalMilliseconds => Milliseconds + Seconds * 1000 + Minutes * 60 * 1000 + Hours * 60 * 60 * 1000;
    public int Milliseconds { get; set; } = milliseconds;
    public int Seconds { get; set; } = seconds;
    public int Minutes { get; set; } = minutes;
    public int Hours { get; set; } = hours;

    public static IRaceTime FromMilliseconds(int totalMilliseconds)
    {
        var milliseconds = totalMilliseconds % 1000;
        var seconds = (totalMilliseconds / 1000) % 60;
        var minutes = (totalMilliseconds / 1000 / 60) % 60;
        var hours = (totalMilliseconds / 1000 / 60 / 60) % 60;

        return new RaceTime(milliseconds, seconds, minutes, hours);
    }

    public override string ToString()
    {
        var milli = Milliseconds.ToString(CultureInfo.InvariantCulture).PadLeft(3, '0');
        var seconds = Seconds.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');

        if (Minutes == 0 && Hours == 0)
        {
            return $"{seconds}.{milli}";
        }

        var minutes = Minutes.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');

        return Hours == 0 ? $"{minutes}:{seconds}.{milli}" : $"{Hours}:{minutes}:{seconds}.{milli}";
    }
}
