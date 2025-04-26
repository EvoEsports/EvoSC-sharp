using System.Collections.Concurrent;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.RoundRankingModule.Models;

namespace EvoSC.Modules.Official.RoundRankingModule.Utils;

public abstract class RoundRankingUtils
{
    /// <summary>
    /// Determines whether a player in the given list passed the finish line.
    /// </summary>
    /// <param name="checkpoints"></param>
    /// <returns></returns>
    public static bool HasPlayerInFinish(List<CheckpointData> checkpoints) =>
        checkpoints.Exists(checkpoint => checkpoint.IsFinish);

    /// <summary>
    /// Determines the winning team based on the given checkpoint data.
    /// </summary>
    /// <param name="checkpoints"></param>
    /// <returns></returns>
    public static PlayerTeam GetWinnerTeam(List<CheckpointData> checkpoints)
    {
        var gainedPointsPerTeam = new Dictionary<PlayerTeam, int>
        {
            { PlayerTeam.Unknown, 0 }, { PlayerTeam.Team1, 0 }, { PlayerTeam.Team2, 0 }
        };

        foreach (var cpData in checkpoints)
        {
            gainedPointsPerTeam[cpData.Player.Team] += cpData.GainedPoints;
        }

        if (gainedPointsPerTeam[PlayerTeam.Team1] == gainedPointsPerTeam[PlayerTeam.Team2])
        {
            return PlayerTeam.Unknown;
        }

        return gainedPointsPerTeam.Where(tp => tp.Value > 0)
            .OrderByDescending(tp => tp.Value)
            .Select(tp => tp.Key)
            .FirstOrDefault(PlayerTeam.Unknown);
    }

    /// <summary>
    /// Calculates the differences in time for each player compared to the leading player.
    /// The calculated times are set on the objects in the list.
    /// </summary>
    /// <param name="checkpoints"></param>
    public static void CalculateAndSetTimeDifferenceOnResult(List<CheckpointData> checkpoints)
    {
        if (checkpoints.Count <= 1)
        {
            return;
        }

        var firstEntry = checkpoints.FirstOrDefault();
        if (firstEntry == null)
        {
            return;
        }

        firstEntry.TimeDifference = null;
        foreach (var cpData in checkpoints[1..])
        {
            cpData.TimeDifference = cpData.GetTimeDifferenceAbsolute(firstEntry);
        }
    }

    /// <summary>
    /// Use the players team color as the accent color for their gained points in the widget.
    /// </summary>
    /// <param name="checkpoints"></param>
    /// <param name="teamColors"></param>
    public static void ApplyTeamColorsAsAccentColors(List<CheckpointData> checkpoints,
        ConcurrentDictionary<PlayerTeam, string> teamColors)
    {
        foreach (var cpData in checkpoints.Where(checkpoint => checkpoint.GainedPoints > 0))
        {
            cpData.AccentColor = teamColors[cpData.Player.Team];
        }
    }

    /// <summary>
    /// Takes the points repartition and applies the gained points to the checkpoint data list.
    /// </summary>
    /// <param name="checkpoints"></param>
    /// <param name="currentPointsRepartition"></param>
    /// <param name="accentColor"></param>
    public static void SetGainedPointsOnResult(List<CheckpointData> checkpoints, PointsRepartition currentPointsRepartition,
        string accentColor)
    {
        var rank = 1;
        foreach (var cpData in checkpoints.Where(checkpoint => checkpoint.IsFinish))
        {
            cpData.AccentColor = accentColor;
            cpData.GainedPoints = currentPointsRepartition.GetGainedPoints(rank++);
        }
    }
}
