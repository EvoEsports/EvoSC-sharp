using EvoSC.Modules.Official.MotdModule.Models;
using EvoSC.Modules.Official.MotdModule.Services;
using Microsoft.Extensions.Logging;

namespace MotdModule.Tests;

public class HttpServiceTests
{
    private const string TestServerUri = "https://directus.evoesports.de/items/motd?filter[server][_eq]=testserver";
    private readonly HttpService _service = new(new Logger<HttpService>(new LoggerFactory()));

    [Theory]
    [InlineData(TestServerUri)]
    [InlineData("https://www.google.com")]
    private async Task Get_Async_Test(string uri)
    {
        var result = await _service.GetAsync(uri);
        Assert.Equal(uri.Contains("evo", StringComparison.InvariantCulture) ? "This is a MOTD message served by the API. Including $f00styling." : "",
            result);
    }

    [Fact]
    private async Task Data_Model_Test()
    {
        var responseObject = new MotdResponse
        {
            Data = new()
            {
                new ResponseData
                {
                    Id = 1, Message = "test message", Server = "testServer"
                }
            }
        };
        Assert.NotNull(responseObject);
        var firstElement = responseObject.Data.FirstOrDefault();
        
        Assert.NotNull(firstElement);
        Assert.Equal(1, firstElement.Id);
        Assert.Equal("test message", firstElement.Message);
        Assert.Equal("testServer", firstElement.Server);
    }

    [Fact]
    private async Task Dispose_Test()
    {
        await _service.GetAsync("https://asd");
        _service.Dispose();
        Assert.True(_service.IsDisposed);
    }
}
