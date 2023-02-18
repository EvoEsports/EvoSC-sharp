namespace EvoSC.Modules.Official.FastestCp;

public record CpTime(int RaceTime, int Time)
{

    public static readonly CpTimeComparator Comparator = new();
    
    public static CpTime Min(CpTime obj, CpTime other)
    {
        return CompareTo(obj, other) != 1 ? obj : other;
    }

    private static int CompareTo(CpTime obj, CpTime other)
    {
        return obj.RaceTime.CompareTo(other.RaceTime) switch
        {
            0 => obj.Time.CompareTo(other.Time),
            var x => x
        };
    }

    public sealed class CpTimeComparator : IComparer<CpTime>
    {
        public int Compare(CpTime? x, CpTime? y)
        {
            return (x, y) switch
            {
                (null, null) => 0,
                (null, _) => 1,
                (_, null) => -1,
                _ => CompareTo(x, y)
            };
        }
    }
}
