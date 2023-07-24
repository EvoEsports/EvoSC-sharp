using EvoSC.Modules.Official.MotdModule.Models;
using EvoSC.Modules.Official.MotdModule.Services;
using Microsoft.Extensions.Logging;
using EvoSC.Testing;
using MockHttpClient;
using Moq;

namespace MotdModule.Tests;

public class HttpServiceTests : IDisposable
{
    private readonly HttpService _service;
    private readonly MockHttpClient.MockHttpClient httpMock;
    private readonly Mock<ILogger<HttpService>> _logger = new();

    public HttpServiceTests()
    {
        httpMock = new MockHttpClient.MockHttpClient();
        _service = new(_logger.Object);
        _service.SetHttpClient(httpMock);
    }

    public static IEnumerable<object[]> GetAsync_Data()
    {
        yield return new object[]
        {
            "https://www.correctUrl.com",
            new HttpResponseMessage()
                .WithStringContent("{\"data\":[{\"id\":2,\"message\":\"This is a MOTD message served by the API. Including $f00styling.\",\"server\":\"testserver\"}]}"),
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
        
        yield return new object[]
        {
            "https://www.falseUrl.com",
            new HttpResponseMessage(),
            new MotdResponse(),
            false
        };
        
        yield return new object[]
        {
            "falseUrl",
            new HttpResponseMessage()
                .WithJsonContent(new
                {
                    WrongData = 1
                }),
            new MotdResponse(),
            true
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
            _logger.Verify(LogLevel.Error,null, It.IsAny<string?>(), Times.Once());
        }
        else
        {
            if (expectedResponse.Data is null)
            {
                Assert.Empty(result);
            }
            else
            {
                var data = expectedResponse.Data.FirstOrDefault()!;
                Assert.Equal(data.Message, result);
                Assert.IsType<int>(data.Id);
                Assert.NotEmpty(data.Server);
            }
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
