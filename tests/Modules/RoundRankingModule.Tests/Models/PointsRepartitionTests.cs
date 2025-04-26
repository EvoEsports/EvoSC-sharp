using EvoSC.Modules.Official.RoundRankingModule.Models;

namespace EvoSC.Modules.Official.RoundRankingModule.Tests.Models;

public class PointsRepartitionTests
{
    [Theory]
    [InlineData("1,2,3,4,5,6,7", new[] { 1, 2, 3, 4, 5, 6, 7 })]
    [InlineData("10,6,4,3,2,1", new[] { 10, 6, 4, 3, 2, 1 })]
    [InlineData("0, 0, 1, 2, -4, -5", new[] { 0, 0, 1, 2, -4, -5 })]
    public void Sets_Points_From_String(string pointsRepartition, int[] expectedPoints)
    {
        Assert.Equal(expectedPoints, new PointsRepartition(pointsRepartition).ToArray());
    }

    [Fact]
    public void Initializes_PointsRepartition_With_Default()
    {
        var expected = PointsRepartition.DefaultValue.Split(',').Select(int.Parse).ToList();

        Assert.Equal(expected, new PointsRepartition());
    }

    [Theory]
    [InlineData("10,6,4,3,2,1", 1, 10)]
    [InlineData("10,6,4,3,2,1", 2, 6)]
    [InlineData("10,6,4,3,2,1", 3, 4)]
    [InlineData("10,6,4,3,2,1", 4, 3)]
    [InlineData("10,6,4,3,2,1", 5, 2)]
    [InlineData("10,6,4,3,2,1", 6, 1)]
    [InlineData("10,6,4,3,2,1", 7, 1)]
    [InlineData("-1,-2,0,7,3", 1, -1)]
    [InlineData("-1,-2,0,7,3", 2, -2)]
    [InlineData("-1,-2,0,7,3", 3, 0)]
    [InlineData("-1,-2,0,7,3", 4, 7)]
    [InlineData("-1,-2,0,7,3", 5, 3)]
    [InlineData("-1,-2,0,7,3", 6, 3)]
    public void Gets_Gained_Points_Correctly(string pointsRepartition, int rank, int expectedGainedPoints)
    {
        Assert.Equal(expectedGainedPoints, new PointsRepartition(pointsRepartition).GetGainedPoints(rank));
    }
}
