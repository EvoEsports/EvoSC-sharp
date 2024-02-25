using EvoSC.Modules.Official.MotdModule.Models;
using EvoSC.Modules.Official.MotdModule.Services;
using EvoSC.Testing;
using Microsoft.Extensions.Logging;
using MockHttpClient;
using NSubstitute;

namespace MotdModule.Tests;

public class HttpServiceTests : IDisposable
{
    private readonly HttpService _service;
    private readonly MockHttpClient.MockHttpClient httpMock;
    private readonly ILogger<HttpService> _logger = Substitute.For<ILogger<HttpService>>();

    public HttpServiceTests()
    {
        httpMock = new MockHttpClient.MockHttpClient();
        _service = new(_logger);
        _service.SetHttpClient(httpMock);
    }

    public static IEnumerable<object[]> GetAsync_Data()
    {
        yield return new object[]
        {
            "https://www.correctUrl.com", new HttpResponseMessage()
                .WithStringContent(
                    "{\"data\":[{\"id\":2,\"message\":\"This is a MOTD message served by the API. Including $f00styling.\",\"server\":\"testserver\"}]}"),
            new MotdResponse
            {
                Data = new List<ResponseData>
                {
                    new ResponseData
                    {
                        Id = 2,
                        Message = "This is a MOTD message served by the API. Including $f00styling.",
                        Server = "testserver"
                    }
                }
            },
            false
        };

        yield return new object[] { "https://www.falseUrl.com", new HttpResponseMessage(), new MotdResponse(), false };

        yield return new object[]
        {
            "falseUrl", new HttpResponseMessage()
                .WithJsonContent(new { WrongData = 1 }),
            new MotdResponse(), true
        };
    }

    [Theory]
    [MemberData(nameof(GetAsync_Data))]
    public async Task GetAsync_Returns_Correct_Format(string url, HttpResponseMessage expectedResult,
        MotdResponse expectedResponse, bool throwException)
    {
        if (!throwException)
        {
            httpMock.When(url)
                .Then(ret => expectedResult);
        }

        var result = await _service.GetAsync(url);
        if (throwException)
        {
            Assert.Empty(result);
            _logger.Received(1).Log(LogLevel.Error, null, Arg.Any<string?>());
        }
        else
        {
            var data = expectedResponse.Data.FirstOrDefault()!;
            Assert.Equal(data.Message, result);
            Assert.IsType<int>(data.Id);
            Assert.NotEmpty(data.Server);
        }
    }

    [Fact]
    public async Task Dispose_Test()
    {
        await _service.GetAsync("https://asd");
        _service.Dispose();
        Assert.True(_service.IsDisposed);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        _service.Dispose();
        httpMock.Dispose();
    }
}
