namespace EvoSC.Modules.Official.RoundRankingModule.Models;

/// <summary>
/// This class is used to easily access the currently set points repartition on the dedicated server and
/// to calculate gained points based on it.
/// </summary>
public class PointsRepartition : List<int>
{
    public static readonly string ModeScriptSetting = "S_PointsRepartition";
    public static readonly string DefaultValue = "10,6,4,3,2,1";

    public PointsRepartition()
    {
        Update(DefaultValue);
    }

    public PointsRepartition(string pointsRepartitionString)
    {
        Update(pointsRepartitionString);
    }

    /// <summary>
    /// Consumes new a points repartition.
    /// Values are comma separated.
    /// </summary>
    /// <param name="pointsRepartitionString"></param>
    public void Update(string pointsRepartitionString)
    {
        Clear();
        AddRange(pointsRepartitionString.Split(',').Select(int.Parse).ToList());
    }

    /// <summary>
    /// Returns the gained points for the given rank.
    /// </summary>
    /// <param name="rank"></param>
    /// <returns></returns>
    public int GetGainedPoints(int rank)
    {
        return rank <= Count ? this[rank - 1] : this.LastOrDefault(0);
    }
}
