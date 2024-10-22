namespace EvoSC.Modules.Official.RoundRankingModule.Models;

public class PointsRepartition : List<int>
{
    public const string ModeScriptSetting = "S_PointsRepartition";
    private const string DefaultValue = "10,6,4,3,2,1";

    public PointsRepartition()
    {
        Update(DefaultValue);
    }

    public void Update(string pointsRepartitionString)
    {
        Clear();
        AddRange(pointsRepartitionString.Split(',').Select(int.Parse).ToList());
    }

    public int GetGainedPoints(int rank)
    {
        return rank <= Count ? this[rank - 1] : this.LastOrDefault(0);
    }
}
