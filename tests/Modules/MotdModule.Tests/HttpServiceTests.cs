using EvoSC.Modules.Official.MotdModule.Models;
using EvoSC.Modules.Official.MotdModule.Services;
using Microsoft.Extensions.Logging;

namespace MotdModule.Tests;

public class HttpServiceTests
{
    private const string TestServerUri = "https://directus.evoesports.de/items/motd?filter[server][_eq]=testserver";
    private readonly HttpService _service = new(new Logger<HttpService>(new LoggerFactory()));

    [Theory]
    [InlineData(TestServerUri, "This is a MOTD message served by the API. Including $f00styling.")]
    [InlineData("https://www.google.com", "")]
    public async Task GetAsync_Returns_Correct_Format(string uri, string expectedResult)
    {
        var result = await _service.GetAsync(uri);
        Assert.Equal(expectedResult,result);
    }

    [Fact]
    public async Task DataModel_MotdResponse_Creation_Successful()
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
    public async Task Dispose_Test()
    {
        await _service.GetAsync("https://asd");
        _service.Dispose();
        Assert.True(_service.IsDisposed);
    }
}
