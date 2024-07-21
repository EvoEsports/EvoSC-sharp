namespace EvoSC.Modules.Official.TeamInfoModule;

public class ModeScriptTeamSettings
{
    public required int PointsLimit { init; get; }
    public required int PointsGap { init; get; }
    public required int RoundsPerMap { init; get; }

    public bool IsTennisMode()
    {
        return PointsGap > 1;
    }
}
