using EvoSC.Modules.Official.OpenPlanetModule.Models;

namespace OpenPlanetModule.Tests.Models;

public class OpenPlanetInfoTests
{
    [Fact]
    public void ToolInfo_Is_Parsed_Correctly()
    {
        var expectedVersion = new Version(12, 34, 56);
        var toolInfo = "Openplanet 12.34.56 (next, Public, 1234-56-78) [OFFICIAL]";

        var opInfo = OpenPlanetInfo.Parse(toolInfo);
        
        Assert.True(opInfo.IsOpenPlanet);
        Assert.Equal(expectedVersion, opInfo.Version);
        Assert.Equal("next", opInfo.Game);
        Assert.Equal("Public", opInfo.Branch);
        Assert.Equal("1234-56-78", opInfo.Build);
        Assert.Equal(OpenPlanetSignatureMode.Official, opInfo.SignatureMode);
    }

    [Fact]
    public void Non_Openplanet_Toolinfo_Returns_Default_And_No_Openplanet_Flag_Set()
    {
        var expectedVersion = new Version(0, 0, 0);

        var opInfo = OpenPlanetInfo.Parse("");
        
        Assert.False(opInfo.IsOpenPlanet);
        Assert.Equal(expectedVersion, opInfo.Version);
        Assert.Equal("", opInfo.Game);
        Assert.Equal("", opInfo.Branch);
        Assert.Equal("", opInfo.Build);
        Assert.Equal(OpenPlanetSignatureMode.Unknown, opInfo.SignatureMode);
    }
}
