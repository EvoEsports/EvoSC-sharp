using EvoSC.Modules.Official.MotdModule.Models;
using EvoSC.Modules.Official.MotdModule.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MotdModule.Tests;

public class HttpServiceTests
{
    private const string TestServerUri = "https://directus.evoesports.de/items/motd?filter[server][_eq]=testserver";
    private readonly HttpService _service = new(new Logger<HttpService>(new LoggerFactory()));

    [Theory]
    [InlineData(TestServerUri)]
    [InlineData("https://www.google.de")]
    private async Task GetAsync_Test(string uri)
    {
        var result = await _service.GetAsync(uri);
        Assert.Equal(uri.Contains("evo", StringComparison.InvariantCulture) ? "This is a MOTD message served by the API. Including $f00styling." : "",
            result);
    }

    [Fact]
    private async Task DataModel_Test()
    {
        var httpClient = new HttpClient();
        using HttpResponseMessage response = await httpClient.GetAsync(new Uri(TestServerUri));
        var result = await response.Content.ReadAsStringAsync();
        MotdResponse? responseObject = null;

        responseObject = JsonConvert.DeserializeObject<MotdResponse>(result);

        Assert.NotNull(responseObject);
        
        Assert.Equal(2, responseObject.data.FirstOrDefault()!.id);
        Assert.Equal("This is a MOTD message served by the API. Including $f00styling.", responseObject.data.FirstOrDefault()!.message);
        Assert.Equal("testserver", responseObject.data.FirstOrDefault()!.server);
    }

    [Fact]
    private async Task Dispose_Test()
    {
        await _service.GetAsync("https://asd");
        _service.Dispose();
        Assert.True(_service.IsDisposed);
    }
}
