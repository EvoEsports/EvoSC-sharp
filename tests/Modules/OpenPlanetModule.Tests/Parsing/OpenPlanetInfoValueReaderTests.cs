using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Parsing;

namespace OpenPlanetModule.Tests.Parsing;

public class OpenPlanetInfoValueReaderTests
{
    [Fact]
    public async Task Reader_Returns_OpenplanetInfo_Object_From_Input()
    {
        var expectedVersion = new Version(12, 34, 56);
        var reader = new OpenPlanetInfoValueReader();
        var input = "Openplanet%2012%2E34%2E56%20%28next%2C%20Public%2C%201234%2D56%2D78%29%20%5BOFFICIAL%5D";

        var opInfo = await reader.ReadAsync(typeof(IOpenPlanetInfo), input) as IOpenPlanetInfo;
        
        Assert.NotNull(opInfo);
        Assert.Equal(expectedVersion, opInfo.Version);
        Assert.Equal("next", opInfo.Game);
        Assert.Equal("Public", opInfo.Branch);
        Assert.Equal("1234-56-78", opInfo.Build);
        Assert.Equal(OpenPlanetSignatureMode.Official, opInfo.SignatureMode);
    }
}
