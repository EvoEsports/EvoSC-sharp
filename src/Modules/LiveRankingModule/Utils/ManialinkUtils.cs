namespace EvoSC.Modules.Official.LiveRankingModule.Utils;

public static class ManialinkUtils
{
    public static string GetPositionColor(IEnumerable<string> colors, int position) => position switch 
    {
        1 => colors.First(),
        2 => colors.Skip(1).First(),
        3 => colors.Skip(2).First(),
        > 3 => colors.Skip(3).First(),
        < 1 => throw new InvalidOperationException("Position cannot be lower than 1.")
    };
}
