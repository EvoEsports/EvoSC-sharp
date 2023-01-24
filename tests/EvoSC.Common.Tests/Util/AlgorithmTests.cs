using EvoSC.Common.Util.Algorithms;
using Xunit;

namespace EvoSC.Common.Tests.Util;

public class AlgorithmTests
{
    [Theory]
    [InlineData("aaaa", "aabb", 2)]
    [InlineData("", "aaaa", 4)]
    [InlineData("aaaa", "", 4)]
    [InlineData("", "", 0)]
    [InlineData("aaaa", "aaaa", 0)]
    [InlineData("aaaa", "aaa", 1)]
    [InlineData("aaa", "aaaa", 1)]
    [InlineData("aaaa", "bba", 3)]
    [InlineData("bba", "aaaa", 3)]
    public void EditDistance_Returns_Correct(string search, string text, int expectedDistance)
    {
        var distance = StringEditDistance.GetDistance(search, text);
        
        Assert.Equal(expectedDistance, distance);
    }
}
