using EvoSC.Commands.Interfaces;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MotdModule.Database.Models;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Modules.Official.MotdModule.Services;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;

namespace MotdModule.Tests;

public class MotdServiceTests
{
    private readonly IManialinkManager _maniaLinkManager = Substitute.For<IManialinkManager>();
    private readonly IHttpService _httpService = Substitute.For<IHttpService>();
    private readonly IMotdRepository _repository = Substitute.For<IMotdRepository>();
    private readonly IMotdSettings _settings = Substitute.For<IMotdSettings>();
    private readonly ILogger<MotdService> _logger = Substitute.For<ILogger<MotdService>>();
    private readonly IOnlinePlayer _player = Substitute.For<IOnlinePlayer>();
    private readonly IPlayerManagerService _playerManager = Substitute.For<IPlayerManagerService>();
    private readonly IContextService _context;

    private const int Max = 100;

    private readonly ControllerContextMock<ICommandInteractionContext> _commandContext =
        Mocking.NewControllerContextMock<ICommandInteractionContext>();

    private MotdService? _motdService;

    public MotdServiceTests()
    {
        _context = Mocking.NewContextServiceMock(_commandContext.Context, null);
    }

    private void SetupMocks(int interval = 200)
    {
        _httpService.GetAsync(Arg.Any<string>()).Returns(Task.FromResult("test"));
        _settings.MotdFetchInterval.Returns(interval);
    }

    private void SetupController()
    {
        _motdService = new(_maniaLinkManager, _httpService, _repository, _settings,
            _logger, _context, _playerManager);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(false, true)]
    public async Task SetMotdSource_Sets_Correct_Source(bool local, bool presetUseLocal = false)
    {
        SetupMocks();
        if (presetUseLocal)
        {
            _settings.UseLocalMotd.Returns(true);
        }

        SetupController();
        await Task.Delay(1000);
        if (!presetUseLocal)
        {
            _settings.UseLocalMotd.Returns(true);
        }

        _httpService.GetAsync(Arg.Any<string>())
            .Returns(Task.FromResult("test"));
        _motdService?.SetMotdSource(local, _player);
        _httpService.ClearReceivedCalls();
        await Task.Delay(300);
        await _httpService.Received(local ? 0 : 1).GetAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task SetLocalMotd_Sets_Locally_Stored_Motd_Correctly()
    {
        SetupMocks();
        SetupController();
        var a = new { text = "test" };
        _maniaLinkManager.SendManialinkAsync(_player, "MotdModule.MotdEdit", a)
            .Returns(Task.CompletedTask);

        _settings.MotdLocalText.Returns("test");

        _motdService?.SetLocalMotd("test", _player);

        await _motdService!.ShowEditAsync(_player);
        await _maniaLinkManager.Received(1).SendManialinkAsync(_player, "MotdModule.MotdEdit", Arg.Any<object>());

        var motdText = await _motdService.GetMotdAsync();
        Assert.Equal(a.text, motdText);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(6)]
    public async Task SetInterval_Timer_Fires_Given_Amount_Of_Times(int times)
    {
        SetupMocks();
        SetupController();
        _motdService!.SetInterval(100, _player);
        await Task.Delay(150 * times);
        await _httpService.Received(Quantity.Within(times, Max)).GetAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task SetUrl_Sets_Url_Correctly_And_Timer_Fires()
    {
        SetupMocks();
        SetupController();
        _httpService.GetAsync("test").Returns(Task.FromResult("test"));
        _motdService!.SetInterval(1000, _player);
        _motdService.SetUrl("test", _player);
        await Task.Delay(1000);

        await _httpService.Received(2).GetAsync("test");
    }

    [Fact]
    public async Task GetEntry_Gets_Correct_Entry()
    {
        SetupMocks();
        SetupController();
        var dbPlayer = new DbPlayer(_player);
        _repository.GetEntryAsync(_player)!
            .Returns(Task.FromResult(new MotdEntry { Hidden = true, DbPlayer = dbPlayer, PlayerId = 1 }));
        var entry = await _motdService!.GetEntryAsync(_player) as MotdEntry;

        await _repository.Received(1).GetEntryAsync(_player);
        Assert.True(entry!.Hidden);
        Assert.Equal(dbPlayer, entry.DbPlayer);
        Assert.Equal(1, entry.PlayerId);
        Assert.Equal(_player.Id, entry.Player.Id);
    }

    [Fact]
    public async Task InsertOrUpdateEntry_Sets_Hidden_State()
    {
        SetupMocks();
        SetupController();
        _repository.InsertOrUpdateEntryAsync(_player, true)
            .Returns(Task.FromResult(new MotdEntry()));
        await _motdService!.InsertOrUpdateEntryAsync(_player, true);

        await _repository.Received(1).InsertOrUpdateEntryAsync(_player, true);
    }

    [Fact]
    public async Task SetUrlReEnable_ReEnables_Timer()
    {
        SetupMocks();
        SetupController();
        _httpService.GetAsync(Arg.Any<string>())
            .ThrowsAsync(new InvalidOperationException());

        await Task.Delay(1000);
        await _httpService.Received(Quantity.AtLeastOne()).GetAsync(Arg.Any<string>());
        _motdService!.SetUrl("test", _player);
        _httpService.GetAsync(Arg.Any<string>())
            .Returns(Task.FromResult("test"));
        await Task.Delay(300);

        await _httpService.Received(Quantity.Within(3, int.MaxValue)).GetAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task GetMotd_Gets_Correct_Text()
    {
        _httpService.GetAsync(Arg.Any<string>())
            .Returns(Task.FromResult("test"));

        SetupMocks();
        SetupController();
        await _motdService!.ShowAsync(_player, true);

        await _maniaLinkManager.Received(1).SendManialinkAsync(_player, "MotdModule.MotdTemplate", Arg.Any<object>());
    }

    [Fact]
    public async Task GetMotd_Trows_Error()
    {
        _httpService.GetAsync(Arg.Any<string>()).ThrowsAsync(new InvalidOperationException());

        SetupController();
        await _motdService!.GetMotdAsync();

        await _httpService.Received(1).GetAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task ShowAsync_Shows_Motd_Manialink()
    {
        _maniaLinkManager.SendManialinkAsync(_player, "MotdModule.MotdTemplate",
            Arg.Any<object>()).Returns(Task.CompletedTask);

        SetupMocks();
        SetupController();
        await Task.Delay(500);
        await _motdService!.ShowAsync(_player, true);

        await _maniaLinkManager.Received(1).SendManialinkAsync(_player, "MotdModule.MotdTemplate",
            Arg.Any<object>());
    }

    [Fact]
    public async Task ShowAsyncLogin_Shows_Motd_Manialink()
    {
        _maniaLinkManager.SendManialinkAsync(_player, "MotdModule.MotdTemplate",
            Arg.Any<object>()).Returns(Task.CompletedTask);
        _playerManager.GetPlayerAsync(Arg.Any<string>())
            .Returns(Task.FromResult((IPlayer?)_player));

        SetupMocks();
        SetupController();
        await Task.Delay(500);
        await _motdService!.ShowAsync("F4aNYLSUS4iB3_Td_a4c8Q", true);

        await _maniaLinkManager.Received(1).SendManialinkAsync(_player, "MotdModule.MotdTemplate",
            Arg.Any<object>());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ShowAsyncNotExplicitly_Shows_Manialink_Depending_On_Hidden_State(bool isHidden)
    {
        _maniaLinkManager.SendManialinkAsync(_player, "MotdModule.MotdTemplate",
            Arg.Any<object>()).Returns(Task.CompletedTask);
        _repository.GetEntryAsync(Arg.Any<IPlayer>())!
            .Returns(Task.FromResult(new MotdEntry { Hidden = isHidden }));

        SetupMocks();
        SetupController();
        await Task.Delay(500);
        await _motdService!.ShowAsync(_player, false);

        await _maniaLinkManager.Received(isHidden ? 0 : 1).SendManialinkAsync(_player, "MotdModule.MotdTemplate",
            Arg.Any<object>());
    }

    [Fact]
    public async Task ShowAsync_Abort_Player_Null()
    {
        SetupMocks();
        SetupController();
        await Task.Delay(500);
        await _motdService!.ShowAsync((IPlayer)null, false);

        await _maniaLinkManager.DidNotReceive().SendManialinkAsync(_player, "MotdModule.MotdTemplate",
            Arg.Any<object>());
    }

    [Fact]
    public void Dispose_Test()
    {
        SetupController();
        _motdService?.Dispose();
        Assert.NotNull(_motdService);
        Assert.True(_motdService.IsDisposed);
    }
}
