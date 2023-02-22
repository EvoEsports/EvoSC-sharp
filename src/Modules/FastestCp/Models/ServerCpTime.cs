using System.Diagnostics.CodeAnalysis;

namespace EvoSC.Modules.Official.FastestCp.Models;

public record ServerCpTime(int RaceTime, int Time) : IComparable<ServerCpTime>
{
    public int CompareTo(ServerCpTime? other)
    {
        return RaceTime.CompareTo(other!.RaceTime) switch
        {
            0 => Time.CompareTo(other.Time),
            var x => x
        };
    }
}
