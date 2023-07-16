using EvoSC.Commands.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MotdModule.Database.Models;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Modules.Official.MotdModule.Services;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using Microsoft.Extensions.Logging;
using Moq;

namespace MotdModule.Tests;

public class MotdServiceTests
{
    private readonly Mock<IManialinkManager> _maniaLinkManager = new();
    private readonly Mock<IHttpService> _httpService = new();
    private readonly Mock<IMotdRepository> _repository = new();
    private readonly Mock<IMotdSettings> _settings = new();
    private readonly Mock<ILogger<MotdService>> _logger = new();
    private readonly Mock<IOnlinePlayer> _player = new();
    private readonly Mock<IContextService> _context;
    
    private readonly ControllerContextMock<ICommandInteractionContext> _commandContext = Mocking.NewControllerContextMock<ICommandInteractionContext>();

    
    
    private MotdService? _motdService;

    public MotdServiceTests()
    {
        _context = Mocking.NewContextServiceMock(_commandContext.Context.Object, null);
    }

    private void SetupMocks(int interval = 200)
    {
        _httpService.Setup(r => r.GetAsync(It.IsAny<string>()))
            .Returns(Task.FromResult("test"));
        _settings.Setup(r => r.MotdFetchInterval)
            .Returns(interval);
    }

    private void SetupController()
    {
        _motdService = new(_maniaLinkManager.Object, _httpService.Object, _repository.Object, _settings.Object,
            _logger.Object, _context.Object);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(false, true)]
    async Task SetMotdSource_Test(bool local, bool presetUseLocal = false)
    {
        SetupMocks(200);
        if (presetUseLocal)
        {
            _settings.Setup(r => r.UseLocalMotd).Returns(true);
        }
        SetupController();
        await Task.Delay(1000);
        if (!presetUseLocal)
        {
            _settings.Setup(r => r.UseLocalMotd).Returns(true);
        }
        _httpService.Setup(r => r.GetAsync(It.IsAny<string>()))
            .Returns(Task.FromResult("test"));
        _motdService.SetMotdSource(local, _player.Object);
        _httpService.Invocations.Clear();
        await Task.Delay(300);
        _httpService.Verify(r => r.GetAsync(It.IsAny<string>()), (local) ? Times.Never : Times.Once);
    }

    [Fact]
    async Task SetLocalMotdTest()
    {
        SetupMocks();
        SetupController();
        var a = new { text = "test" };
        _maniaLinkManager.Setup(r =>
            r.SendManialinkAsync(_player.Object, "MotdModule.MotdEdit", a))
            .Returns(Task.CompletedTask);
        
        _settings.Setup(r => r.MotdLocalText).Returns("test");
        
        _motdService.SetLocalMotd("test", _player.Object);
        
        await _motdService.ShowEditAsync(_player.Object);
        _maniaLinkManager.Verify(r =>
            r.SendManialinkAsync(_player.Object, "MotdModule.MotdEdit", It.IsAny<object>()), Times.Once);

        var motdText = await _motdService.GetMotdAsync();
        Assert.Equal(a.text, motdText);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    async Task SetIntervalTest(int times)
    {
        SetupMocks();
        SetupController();
        _motdService!.SetInterval(100, _player.Object);
        await Task.Delay(110 * times);
        _httpService.Verify(r => r.GetAsync(It.IsAny<string>()),
            Times.Exactly(times));
    }

    [Fact]
    async Task SetUrlTest()
    {
        SetupMocks();
        SetupController();
        _httpService.Setup(r => r.GetAsync("test")).Returns(Task.FromResult("test"));
        _motdService!.SetInterval(1000, _player.Object);
        _motdService.SetUrl("test", _player.Object);
        await Task.Delay(1000);

        _httpService.Verify(r => r.GetAsync("test"), Times.Exactly(2));
    }

    [Fact]
    async Task GetEntryTest()
    {
        SetupMocks();
        SetupController();
        _repository.Setup(r => r.GetEntryAsync(_player.Object))
            .Returns(Task.FromResult(new MotdEntry())!);
        await _motdService!.GetEntryAsync(_player.Object);
        
        _repository.Verify(r => r.GetEntryAsync(_player.Object), Times.Once);
    }
    
    [Fact]
    async Task InsertOrUpdateEntryTest()
    {
        SetupMocks();
        SetupController();
        _repository.Setup(r => r.InsertOrUpdateEntryAsync(_player.Object, true))
            .Returns(Task.FromResult(new MotdEntry()));
        await _motdService!.InsertOrUpdateEntryAsync(_player.Object, true);
        
        _repository.Verify(r => r.InsertOrUpdateEntryAsync(_player.Object, true), Times.Once);
    }

    [Fact]
    async Task SetUrlReEnableTest()
    {
        SetupMocks();
        SetupController();
        _httpService.Setup(r => r.GetAsync(It.IsAny<string>()))
            .Throws(new InvalidOperationException());
        
        await Task.Delay(1000);
        _httpService.Verify(r => r.GetAsync(It.IsAny<string>()), Times.AtLeast(1));
        _motdService!.SetUrl("test", _player.Object);
        _httpService.Setup(r => r.GetAsync(It.IsAny<string>()))
            .Returns(Task.FromResult("test"));
        await Task.Delay(300);
        _httpService.Verify(r => r.GetAsync(It.IsAny<string>()), Times.AtLeast(3));
    }
    
    [Fact]
    async Task GetMotdTest()
    {
        _httpService.Setup(r => r.GetAsync(It.IsAny<string>()))
            .Returns(Task.FromResult("test"));
        _maniaLinkManager.Setup(r => r.SendManialinkAsync(_player.Object, "MotdModule.MotdTemplate",
            new { isChecked = false, text = "test" }));
        
        SetupMocks();
        SetupController();
        await _motdService!.ShowAsync(_player.Object);
        
        _maniaLinkManager.Verify(r => r.SendManialinkAsync(_player.Object, "MotdModule.MotdTemplate", 
            It.IsAny<object>()), Times.Once);
    }

    [Fact]
    async Task GetMotdThrowTest()
    {
        _httpService.Setup(r => r.GetAsync(It.IsAny<string>()))
            .Throws(new InvalidOperationException());
        
        SetupController();
        await _motdService!.GetMotdAsync();
        
        _httpService.Verify(r => r.GetAsync(It.IsAny<string>()),
            Times.Once);
    }
    
    [Fact]
    async Task ShowAsyncTest()
    {
        _maniaLinkManager.Setup(r => r.SendManialinkAsync(_player.Object, "MotdModule.MotdTemplate",
            It.IsAny<object>())).Returns(Task.CompletedTask);
        
        SetupMocks();
        SetupController();
        await Task.Delay(500);
        await _motdService!.ShowAsync(_player.Object);
        
        _maniaLinkManager.Verify(r => r.SendManialinkAsync(_player.Object, "MotdModule.MotdTemplate", 
            It.IsAny<object>()), Times.Once);
    }

    [Fact]
    async Task DisposeTest()
    {
        SetupController();
        _motdService.Dispose();
        Assert.True(_motdService.IsDisposed);
    } 
}
