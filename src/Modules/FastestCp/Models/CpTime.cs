namespace EvoSC.Modules.Official.FastestCp.Models;

public record CpTime(int RaceTime, int Time): IComparable<CpTime>
{
    public int CompareTo(CpTime? other)
    {
        if (other == null)
        {
            return -1;
        }

        return RaceTime.CompareTo(other.RaceTime) switch
        {
            0 => Time.CompareTo(other.Time),
            var x => x
        };
    }
}
