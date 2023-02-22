using System.Diagnostics.CodeAnalysis;

namespace EvoSC.Modules.Official.FastestCp.Models;

public record ServerCpTime(int RaceTime, int Time) : IComparable<ServerCpTime>
{
    [ExcludeFromCodeCoverage(Justification = "other cannot be null as it is only used implicitly by FastestCpStore that constructs its instances")]
    public int CompareTo(ServerCpTime? other)
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
